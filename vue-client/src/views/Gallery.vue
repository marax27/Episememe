<template>
  <div class='wrapper'>
    <MediaGallery :instances='mediaInstances'></MediaGallery>
    <SettingsMenu></SettingsMenu>
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

  mediaInstances: IMediaInstance[] = [];

  created() {
    this.$api.get<IMediaInstance[]>(`media?q=${this.galleryData}`)
      .then(response => this.mediaInstances = response.data)
      .catch(err => {
        this.$store.dispatch('reportError', 'Failed to load the media.');
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
