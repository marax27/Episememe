<template>
  <div class='wrapper'>
    <MediaGallery :instances='mediaInstances'></MediaGallery>
    <SettingsMenu></SettingsMenu>

    <v-snackbar
      v-model='errorSnackbarIsOpen'
      :timeout='3000'>
      Failed to load the media. Try again later.
      <v-btn
        text color='error'
        @click='errorSnackbarIsOpen = false;'>
        Close
      </v-btn>
    </v-snackbar>
  </div>
</template>

<script lang='ts'>
import { Component, Vue, Mixins } from 'vue-property-decorator';
import MediaGallery from '@/browsing/media-gallery/MediaGallery.vue';
import SettingsMenu from '@/browsing/SettingsMenu.vue';
import { IMediaInstance } from '../shared/models/IMediaInstance';
import ApiClientService from '../shared/mixins/api-client/api-client.service';

@Component({
  components: {
    MediaGallery,
    SettingsMenu
  }
})
export default class Gallery extends Mixins(ApiClientService) {

  errorSnackbarIsOpen = false;

  mediaInstances: IMediaInstance[] = [];

  created() {
    this.$api.get<IMediaInstance[]>(`media?q=${this.galleryData}`)
      .then(response => this.mediaInstances = response.data)
      .catch(err => {
        this.errorSnackbarIsOpen = true;
      });
  }

  public get galleryData() {
    return this.$route.params.data;
  }
}
</script>

<style scoped>
.wrapper {
  width: 100%;
  height: 100%;
  margin: 0;
  padding: 0;
}
</style>
