<template>
  <div class='wrapper'>
    <MediaGallery></MediaGallery>
    <SettingsMenu></SettingsMenu>
  </div>
</template>

<script lang='ts'>
import { Component, Prop, Vue } from 'vue-property-decorator';
import MediaGallery from '@/browsing/MediaGallery.vue';
import SettingsMenu from '@/browsing/SettingsMenu.vue';

@Component({
  components: {
    MediaGallery,
    SettingsMenu
  }
})
export default class Gallery extends Vue {

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
