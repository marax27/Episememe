<template>
  <v-card>
    <v-card-title>Upload</v-card-title>

    <v-card-text>
      <v-row dense align='stretch' justify='space-between'>
        <v-col cols='12' md='6'>
          <div v-ripple class='upload-field elevation-2 pa-1 d-flex flex-column align-center justify-center fill-height'>
            <v-icon class='upload-icon'>mdi-upload</v-icon>
          </div>
        </v-col>

        <v-col cols='12' md='6'>
          <div class='d-flex flex-column align-center justify-space-between fill-height'>
            <v-card class='align-self-stretch' color='secondary darken-1'>
              <v-card-title>Tag Selection</v-card-title>

              <v-card-text>
                ...
              </v-card-text>
            </v-card>

            <v-btn
              color='primary'
              class='ma-1'
              @click='upload'>

              Upload
            </v-btn>
          </div>
        </v-col>
      </v-row>
    </v-card-text>
  </v-card>
</template>

<script lang='ts'>
import { Component, Vue } from 'vue-property-decorator';

@Component
export default class UploadPanel extends Vue {

  upload() {
    return undefined;
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
</style>
