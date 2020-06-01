<template>
  <BaseSettingsMenuButton
    label='Favorite'
    :icon='isFavorite ? "mdi-star" : "mdi-star-outline"'
    :color='isFavorite ? "accent" : undefined'
    @click='toggleFavorite' />
</template>

<script lang='ts'>
import { Component, Vue } from 'vue-property-decorator';
import BaseSettingsMenuButton from './BaseSettingsMenuButton.vue';

@Component({
  components: {
    BaseSettingsMenuButton
  }
})
export default class FavoriteMenuButton extends Vue {

  get isFavorite(): boolean {
    return this.$store.state.favorite.favoriteIds.includes(this.currentMediaId);
  }

  toggleFavorite() {
    this.$store.dispatch('favorite/toggleFavoriteId', this.currentMediaId);
  }

  private get currentMediaId(): string {
    return this.$store.getters.currentMediaId;
  }
}
</script>
