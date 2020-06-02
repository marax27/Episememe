<template>
  <BaseSettingsMenuButton
    label='Favorite'
    :icon='isFavorite ? "mdi-star" : "mdi-star-outline"'
    :color='isFavorite ? "accent" : undefined'
    @click='toggleFavorite' />
</template>

<script lang='ts'>
import { Component, Vue, Mixins } from 'vue-property-decorator';
import BaseSettingsMenuButton from './BaseSettingsMenuButton.vue';
import ApiClientService from '../../../shared/mixins/api-client/api-client.service';

@Component({
  components: {
    BaseSettingsMenuButton
  }
})
export default class FavoriteMenuButton extends Mixins(ApiClientService) {

  get isFavorite(): boolean {
    return this.$store.state.favorite.favoriteIds.includes(this.currentMediaId);
  }

  toggleFavorite() {
    const newState = !this.isFavorite;
    this.toggleFavoriteInStore();
    this.toggleFavoriteInApi(newState, this.currentMediaId)
      .catch(_err => {
        this.toggleFavoriteInStore();
        this.$store.dispatch('reportError', 'Failed to update your preferences.');
      });
  }

  private get currentMediaId(): string {
    return this.$store.getters['gallery/currentMediaId'];
  }

  private toggleFavoriteInStore() {
    return this.$store.dispatch('favorite/toggleFavoriteId', this.currentMediaId);
  }

  private toggleFavoriteInApi(newValue: boolean, mediaId: string) {
    const url = 'favorite/' + mediaId;
    if (newValue) {
      return this.$api.put<void>(url, {});
    } else {
      return this.$api.delete<void>(url, {});
    }
  }
}
</script>
