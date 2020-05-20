<template>
  <div class='wrapper'>
    <MediaGallery
      v-if='!isLoading'
      :instances='mediaInstances'></MediaGallery>
    <div v-else class='loading-spinner'>
      <v-progress-circular indeterminate></v-progress-circular>
    </div>

    <SettingsMenu
      @revise='isOpen = true'>
    </SettingsMenu>
    <RevisionPopup
      v-model='isOpen'>
    </RevisionPopup>
  </div>
</template>

<script lang='ts'>
import { Component, Vue, Mixins } from 'vue-property-decorator';
import MediaGallery from '@/browsing/media-gallery/MediaGallery.vue';
import RevisionPopup from '@/browsing/revision-popup/RevisionPopup.vue';
import SettingsMenu from '@/browsing/SettingsMenu.vue';
import { IMediaInstance } from '../shared/models/IMediaInstance';
import ApiClientService from '../shared/mixins/api-client/api-client.service';

@Component({
  components: {
    MediaGallery,
    SettingsMenu,
    RevisionPopup,
  }
})
export default class Gallery extends Mixins(ApiClientService) {

  mediaInstances: IMediaInstance[] = [];

  isOpen = false;
  isLoading = true;

  created() {
    this.refreshBrowseToken(this.loadMedia);
  }

  public get galleryData() {
    return this.$route.params.data;
  }

  private loadMedia() {
    this.$api.get<IMediaInstance[]>(`media?q=${this.galleryData}`)
      .then(response => {
        this.mediaInstances = response.data;
        this.isLoading = false;
      })
      .catch(err => {
        this.$store.dispatch('reportError', 'Failed to load the media.');
        this.isLoading = false;
      });
  }

  private refreshBrowseToken(afterCompletion: () => void) {
    return this.$api
      .post<string>('authorization', {})
      .then(response => {
        this.$store.dispatch('refreshBrowseToken', response.data)
          .then(_ => afterCompletion());
      })
      .catch(err => {
        if (this.$store.state.browseToken != null) {
          afterCompletion();
        } else {
          this.$store.dispatch('reportError', 'Failed to retrieve data from the server.');
        }
      });
  }
}
</script>

<style scoped>
.wrapper {
  position: relative;
  width: 100%;
  height: 100%;
  margin: 0;
  padding: 0;
}

.loading-spinner {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
}
</style>
