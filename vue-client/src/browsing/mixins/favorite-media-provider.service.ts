import { Component, Mixins } from 'vue-property-decorator';
import ApiClientService from '@/shared/mixins/api-client/api-client.service';
import { IMediaInstance } from '@/shared/models/IMediaInstance';

@Component
export default class TagsProviderService extends Mixins(ApiClientService) {

  created() {
    if (this.$store.state.favorite.favoriteIds == null) {
      this._loadFavoriteMedia();
    }
  }

  public refreshTags() {
    this._loadFavoriteMedia();
  }

  private _loadFavoriteMedia() {
    this.$api.get<IMediaInstance[]>('favorite')
      .then(response => response.data)
      .then(mediaInstances => {
        const ids = mediaInstances.map(mi => mi.id);
        return this.$store.dispatch('favorite/updateFavoriteIds', ids);
      }).catch(_err => {
        this.$store.dispatch('reportError', 'Failed to load data from the server.');
      });
  }
}
