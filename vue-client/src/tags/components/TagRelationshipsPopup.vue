<template>
  <v-dialog
    v-model='isOpen'
    width='80%'
    persistent>

    <v-card outlined>
      <v-card-title>
        <v-icon left>mdi-tag-multiple-outline</v-icon>
        Tag Relationships
      </v-card-title>

      <v-card-text class='pa-2'>
        <SingleTagPicker
          v-if='isOpen'
          v-model='selectedTag'
          hint='Pick one of the existing tags to edit it.' />

        <v-text-field
          dense
          label='Description'
          v-model='description'
          hide-details='auto'
          outlined>
        </v-text-field>
      </v-card-text>

      <v-card-actions>
        <v-btn
          color='secondary'
          @click='close'>
          <v-icon left>mdi-close</v-icon> Close
        </v-btn>

        <v-spacer></v-spacer>

        <v-btn
          color='primary'
          @click='applyChanges'>
          <v-icon left>mdi-arrow-up-circle-outline</v-icon> Apply changes
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang='ts'>
import { Component, Vue, Watch } from 'vue-property-decorator';
import SingleTagPicker from './SingleTagPicker.vue';
import { ITag } from '../../shared/models/ITag';

@Component({
  components: {
    SingleTagPicker,
  }
})
export default class TagRelationshipsPopup extends Vue {
  get isOpen(): boolean {
    return this.$store.state.popups.tagRelationships.isOpen;
  }
  set isOpen(newValue: boolean) {
    const actionName = newValue ? 'popups/openTagRelationships' : 'popups/closeTagRelationships';
    this.$store.dispatch(actionName);
  }

  selectedTag: ITag | null = null;
  description?: string = '';

  close() {
    this.isOpen = false;
  }

  applyChanges() {
    this.isOpen = false;
  }

  @Watch('selectedTag')
  onSelectedTagChange() {
    this.description = this.selectedTag?.description;
  }
}
</script>
