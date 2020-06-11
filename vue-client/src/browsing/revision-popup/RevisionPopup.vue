<template>
  <v-dialog
    v-model='isOpen'
    width='70%'
    persistent
    eager>

    <v-card>
      <v-card-title>
        Revision
      </v-card-title>

      <v-card-text>
        Update the list of tags. When you're satisfied, click <span class='font-weight-bold'>Submit</span>.
      </v-card-text>

      <v-card-text>
        <BasicTagPicker
          v-model='tagNames'>
        </BasicTagPicker>
      </v-card-text>

      <v-divider></v-divider>

      <v-card-actions>
        <v-btn
          color='secondary'
          @click='close'>
          <v-icon left>mdi-close</v-icon> Close
        </v-btn>

        <v-btn
          color='secondary'
          @click='reset'>
          <v-icon>mdi-restart</v-icon> Reset tags
        </v-btn>

        <v-spacer></v-spacer>

        <v-btn
          color='primary'
          @click='submit'>
          <v-icon left>mdi-square-edit-outline</v-icon> Submit
        </v-btn>
      </v-card-actions>
    </v-card>

  </v-dialog>
</template>

<script lang='ts'>
import { Component, Prop, Watch, Mixins } from 'vue-property-decorator';
import BasicTagPicker from '@/tags/components/BasicTagPicker.vue';
import { IMediaInstance } from '../../shared/models/IMediaInstance';
import ApiClientService from '../../shared/mixins/api-client/api-client.service';
import TagsProviderService from '../../tags/mixins/tags-provider.service';

@Component({
  components: {
    BasicTagPicker
  }
})
export default class RevisionPopup extends Mixins(ApiClientService, TagsProviderService) {

  tagNames: string[] = [];

  get isOpen(): boolean {
    return this.$store.state.popups.revision.isOpen;
  }
  set isOpen(newValue: boolean) {
    const actionName = newValue ? 'popups/openRevision' : 'popups/closeRevision';
    this.$store.dispatch(actionName);
  }

  get currentInstance(): IMediaInstance {
    return this.$store.state.gallery.currentMediaInstance;
  }

  reset() {
    this.loadInitialTags();
  }

  close() {
    this.isOpen = false;
  }

  submit() {
    const mediaId = this.currentInstance.id;

    this.$api.patch<void>(`media/${mediaId}`, {tags: this.tagNames})
      .then(_response => {
        this.updateLocalMediaInstance();
        this.refreshTags();
        this.close();
      })
      .catch(_err => {
        this.$store.dispatch('reportError', 'Failed to complete the revision.');
      });
  }

  @Watch('isOpen')
  private onIsOpenChange(newValue: boolean) {
    if (newValue) {
      this.loadInitialTags();
    }
  }

  private loadInitialTags() {
    this.tagNames = [...this.currentInstance.tags];
  }

  private updateLocalMediaInstance() {
    this.currentInstance.tags = this.tagNames;
  }
}
</script>
