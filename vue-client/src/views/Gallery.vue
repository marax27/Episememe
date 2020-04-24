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

@Component({
  components: {
    MediaGallery,
    SettingsMenu
  }
})
export default class Gallery extends Vue {

  mediaInstances = [
    { id: '1', address: 'http://localhost:18888/0.jpg' },
    { id: '2', address: 'http://localhost:18888/1.jpg' },
    { id: '3', address: 'http://localhost:18888/2.pdf' },
    { id: '4', address: 'http://localhost:18888/3.jpg' },
    { id: '5', address: 'http://localhost:18888/4.jpg' },
    { id: '6', address: 'http://localhost:18888/5.mp4' },
  ];

  speedDialIsOpen = false;

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
