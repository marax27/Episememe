<template>
  <v-hover v-slot:default='{ hover }'>
    <v-card
      ripple
      tile
      :color='hover ? "secondary" : "secondary darken-1"'
      class='upload-field elevation-2 pa-1 d-flex flex-column align-center justify-center fill-height'>

      <template v-if='isFileProvided()'>
        <p class='font-weight-bold'>{{ selectedFile.name }}</p>
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
  </v-hover>
</template>

<script lang='ts'>
import { Component, Vue } from 'vue-property-decorator';

@Component
export default class UploadTile extends Vue {
  selectedFile: File | null = null;

  handleNewFile() {
    const fe = this.$refs.fileUpload as HTMLInputElement;
    if (fe == null || fe.files == null) {
      return;
    }
    this.selectedFile = fe.files[0];
    this.$emit('input', this.selectedFile);
  }

  isFileProvided(): boolean {
    return this.selectedFile != null;
  }
}
</script>

<style scoped>
.upload-field {
  min-height: 50vh;
  max-height: 90vh;
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
</style>
