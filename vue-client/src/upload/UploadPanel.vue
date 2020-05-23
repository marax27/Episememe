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


            <UploadPanelSecondaryTile
              :disabled='!isFileProvided()'
              title='Tags'
              class='secondary-column-field'>

              <BasicTagPicker v-model='tagNames' />
            </UploadPanelSecondaryTile>

            <UploadPanelSecondaryTile
              :disabled='!isFileProvided()'
              title='Properties'
              class='secondary-column-field'>

              <v-row columns='12' dense>
                <v-col cols='6'>
                  <v-checkbox
                    class='ma-0'
                    v-model='isPrivate'
                    prepend-icon='mdi-account-lock-outline'
                    hint='A private file is not available to other users'
                    persistent-hint
                    label='Mark as private'>
                  </v-checkbox>
                </v-col>
              </v-row>
            </UploadPanelSecondaryTile>

            <UploadPanelSecondaryTile
              :disabled='!isFileProvided()'
              title='Actions'
              class='secondary-column-field'>

              <v-row columns='12' dense>
                <v-col cols='6'>
                  <DeduceTagsTile @click='deduceTags' />
                </v-col>
              </v-row>
            </UploadPanelSecondaryTile>

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
import UploadPanelSecondaryTile from './components/UploadPanelSecondaryTile.vue';
import { FileUploadDto } from './interfaces/FileUploadDto';

@Component({
  components: {
    UploadPanelSecondaryTile,
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

  isPrivate = false;

  updateFile(file: File | null) {
    this.currentFile = file;
    this.uploadButtonLabel = 'Upload';
  }

  isFileProvided(): boolean {
    return this.currentFile != null;
  }

  deduceTags() {
    if (this.currentFile != null) {
      const newTagNames = this.deduceTagsForFile(this.currentFile)
        .filter(name => !this.tagNames.includes(name));
      newTagNames.forEach(name => this.tagNames.push(name));
    }
  }

  upload() {
    if (this.currentFile == null)
      return;

    const headers = { 'Content-Type': 'multipart/form-data' };
    const data = this.createRequestPayload(this.currentFile);

    this.uploadInProgress = true;
    this.$api.post<void>('files', data, headers)
      .then(_response => {
        this.uploadInProgress = false;
        this.currentFile = null;
        this.uploadButtonLabel = 'Uploaded';
        this.refreshTags();
      })
      .catch(_err => {
        this.uploadInProgress = false;
        this.$store.dispatch('reportError', 'Failed to upload the file.');
      });
  }

  private createRequestPayload(file: File): FormData {
    const data = new FormData();
    data.append('File', file as any);

    const mediaDto: FileUploadDto = {
      tags: this.tagNames,
      timestamp: new Date(file.lastModified),
      isPrivate: this.isPrivate
    };
    data.append('Media', JSON.stringify(mediaDto));
    return data;
  }
}
</script>

<style scoped>
.secondary-column-field + .secondary-column-field {
  margin-top: .5em;
}
</style>
