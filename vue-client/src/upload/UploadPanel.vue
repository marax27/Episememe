<template>
  <v-card>
    <v-card-title>Upload</v-card-title>

    <v-card-text>
      <v-row dense align='stretch' justify='space-between'>
        <v-col cols='12' md='4'>
          <UploadTile @input='updateFile' />
        </v-col>

        <v-col cols='12' md='8'>
          <div class='d-flex flex-column align-center justify-space-between fill-height'>
            <v-card
              :disabled='!isFileProvided()'
              tile
              class='secondary-column-field align-self-stretch'
              color='secondary darken-1'>

              <v-card-title>Tag Selection</v-card-title>

              <v-card-text>
                <BasicTagPicker
                  v-model='tagNames'>
                </BasicTagPicker>
              </v-card-text>
            </v-card>

            <v-card
              :disabled='!isFileProvided()'
              tile
              class='secondary-column-field align-self-stretch'
              color='secondary darken-1'>

              <v-card-text>
                <v-row columns='12' dense>

                  <v-col cols='6'>
                    <DeduceTagsTile @click='deduceTags' />
                  </v-col>

                </v-row>
              </v-card-text>
            </v-card>

            <v-btn
              :disabled='!isFileProvided()'
              :loading='uploadInProgress'
              color='primary'
              class='secondary-column-field'
              @click='upload'>

              <v-icon left>mdi-upload</v-icon> {{ uploadButtonLabel }}
            </v-btn>
          </div>
        </v-col>
      </v-row>
    </v-card-text>
  </v-card>
</template>

<script lang='ts'>
import { Component, Mixins } from 'vue-property-decorator';
import ApiClientService from '../shared/mixins/api-client/api-client.service';
import BasicTagPicker from '../tags/components/BasicTagPicker.vue';
import TagsProviderService from '../tags/mixins/tags-provider.service';
import TagsDeductionService from '../tags/mixins/tags-deduction.service';
import DeduceTagsTile from './components/DeduceTagsTile.vue';
import UploadTile from './components/UploadTile.vue';

@Component({
  components: {
    UploadTile,
    BasicTagPicker,
    DeduceTagsTile
  }
})
export default class UploadPanel extends Mixins(ApiClientService, TagsDeductionService, TagsProviderService) {

  currentFile: File | null = null;
  tagNames: string[] = [];
  uploadInProgress = false;
  uploadButtonLabel = 'Upload';

  updateFile(file: File | null) {
    this.currentFile = file;
    this.uploadButtonLabel = 'Upload';
  }

  isFileProvided(): boolean {
    return this.currentFile != null;
  }

  deduceTags() {
    if (this.currentFile != null) {
      this.tagNames = this.deduceTagsForFile(this.currentFile);
    }
  }

  upload() {
    const data = new FormData();
    data.append('File', this.currentFile as any);
    data.append('Tags', JSON.stringify(this.tagNames));
    const headers = { 'Content-Type': 'multipart/form-data' };

    this.uploadInProgress = true;
    this.$api.post<any>('files', data, headers)
      .then(response => {
        this.uploadInProgress = false;
        this.currentFile = null;
        this.uploadButtonLabel = 'Uploaded';
        this.refreshTags();
      })
      .catch(err => {
        this.uploadInProgress = false;
        this.$store.dispatch('reportError', 'Failed to upload the file.');
      });
  }
}
</script>

<style scoped>
.secondary-column-field + .secondary-column-field {
  margin-top: .5em;
}
</style>
