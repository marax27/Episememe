<template>
  <div class='wrapper'>
    <MediaGallery :instances='mediaInstances'></MediaGallery>
    <SettingsMenu></SettingsMenu>
  </div>
</template>

<script lang='ts'>
import { Component, Vue } from 'vue-property-decorator';
import MediaGallery from '@/browsing/media-gallery/MediaGallery.vue';
import SettingsMenu from '@/browsing/SettingsMenu.vue';
import { IMediaInstance } from '../shared/models/IMediaInstance';

@Component({
  components: {
    MediaGallery,
    SettingsMenu
  }
})
export default class Gallery extends Vue {

  mediaInstances: IMediaInstance[] = [
    { id: 'jpg00', dataType: 'jpg', tags: [] },
    { id: 'jpg01', dataType: 'jpg', tags: [] },
    { id: 'pdf02', dataType: 'pdf', tags: [] },
    { id: 'jpg03', dataType: 'jpg', tags: [] },
    { id: 'jpg04', dataType: 'jpg', tags: [] },
    { id: 'mp405', dataType: 'mp4', tags: [] },
  ];

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
