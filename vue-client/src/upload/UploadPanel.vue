<template>
  <v-card>
    <v-card-title>Upload</v-card-title>

    <v-card-text>
      <v-row dense align='stretch' justify='space-between'>
        <v-col cols='12' md='4'>
          <v-card
            ripple
            tile
            class='upload-field elevation-2 pa-1 d-flex flex-column align-center justify-center fill-height'>

            <template v-if='isFileProvided()'>
              <p class='font-weight-bold'>{{ currentFile.name }}</p>
              <p>Click again to load a different file.</p>
            </template>
            <template v-else>
              <v-icon class='upload-icon'>mdi-upload</v-icon>
             <span>Click to load a file</span>
            </template>

            <input
              class='file-input'
              type='file'
              ref='fileUpload'
              @change='handleNewFile' />
          </v-card>
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

@Component({
  components: {
    BasicTagPicker
  }
})
export default class UploadPanel extends Mixins(ApiClientService) {

  currentFile: File | null = null;
  tagNames: string[] = [];
  uploadInProgress = false;
  uploadButtonLabel = 'Upload';

  handleNewFile() {
    const fe = this.$refs.fileUpload as HTMLInputElement;
    if (fe == null || fe.files == null) {
      return;
    }
    this.currentFile = fe.files[0];
    this.uploadButtonLabel = 'Upload';
  }

  isFileProvided(): boolean {
    return this.currentFile != null;
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
      })
      .catch(err => {
        this.uploadInProgress = false;
        this.$store.dispatch('reportError', 'Failed to upload the file.');
      });
  }
}
</script>

<style scoped>
.upload-field {
  min-height: 50vh;
  max-height: 90vh;
  background-color: var(--v-secondary-darken1);
}

.upload-field .upload-icon {
  font-size: 8em;
  flex: 0 1 auto;
}

.upload-field .file-input {
  opacity: 0;
  position: absolute;
  top: 0;
  left: 0;
  bottom: 0;
  right: 0;
  width: 100%;
  height: 100%;
}

.secondary-column-field + .secondary-column-field {
  margin-top: .5em;
}
</style>
