<template>
  <v-dialog v-model='isOpen' width='70%' eager>

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

        <v-spacer></v-spacer>

        <v-btn
          color='primary'
          @click='close'>
          <v-icon left>mdi-square-edit-outline</v-icon> Submit
        </v-btn>
      </v-card-actions>
    </v-card>

  </v-dialog>
</template>

<script lang='ts'>
import { Component, Vue, Prop, Watch } from 'vue-property-decorator';
import BasicTagPicker from '@/tags/components/BasicTagPicker.vue';
import { IMediaInstance } from '../../shared/models/IMediaInstance';

@Component({
  components: {
    BasicTagPicker
  }
})
export default class RevisionPopup extends Vue {

  @Prop({ default: false })
  value!: boolean;

  tagNames: string[] = [];

  get isOpen(): boolean {
    return this.value;
  }
  set isOpen(newValue: boolean) {
    this.$emit('input', newValue);
  }

  get currentInstance(): IMediaInstance {
    return this.$store.state.gallery.currentMediaInstance;
  }

  close() {
    this.isOpen = false;
  }

  @Watch('value')
  private onValueChange(newValue: boolean) {
    if (newValue) {
      this.loadInitialTags();
    }
  }

  private loadInitialTags() {
    this.tagNames = [...this.currentInstance.tags];
  }
}
</script>
