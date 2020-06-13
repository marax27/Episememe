<template>
  <div class='wrapper'>
    <MediaGallery
      v-if='!isLoading'
      :instances='mediaInstances'></MediaGallery>
    <div v-else class='loading-spinner'>
      <v-progress-circular indeterminate></v-progress-circular>
    </div>

    <SettingsMenu />
    <RevisionPopup />
  </div>
</template>

<script lang='ts'>
import { Component, Mixins } from 'vue-property-decorator';
import MediaGallery from '@/browsing/media-gallery/MediaGallery.vue';
import RevisionPopup from '@/browsing/revision-popup/RevisionPopup.vue';
import SettingsMenu from '@/browsing/settings-menu/SettingsMenu.vue';
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

  isLoading = true;

  created() {
    this.refreshBrowseToken()
      .then(_onSuccess => this.loadMedia())
      .then(
        _onSuccess => this.finalizeLoading(),
        _onFailure => {
          this.$store.dispatch('reportError', 'Failed to retrieve data from the server.');
          this.finalizeLoading();
        });
  }

  public get galleryData() {
    return this.$route.params.data;
  }

  private finalizeLoading() {
    this.isLoading = false;
  }

  private loadMedia() {
    return this.$api.get<IMediaInstance[]>(`media?q=${this.galleryData}`)
      .then(response => {
        this.mediaInstances = response.data;
      })
      .catch(_err => {
        this.$store.dispatch('reportError', 'Failed to load the media.');
      });
  }

  private refreshBrowseToken() {
    return this.$api
      .post<string>('authorization', {})
      .then(response => {
        return this.$store.dispatch('refreshBrowseToken', response.data);
      }, _onFailure => {
        // If there is a token in store, suppress the error and try with an existing token.
        if (this.$store.state.browseToken == null) {
          return Promise.reject();
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
