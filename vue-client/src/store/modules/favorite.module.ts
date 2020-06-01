import { Module } from 'vuex';

export const favorite: Module<any, any> = {
  namespaced: true,
  state: () => ({
    favoriteIds: [],
  }),
  mutations: {
    toggleFavoriteId(state, mediaId: string) {
      const index = state.favoriteIds.indexOf(mediaId);
      if (index === -1) {
        state.favoriteIds.push(mediaId);
      } else {
        state.favoriteIds.splice(index, 1);
      }
    }
  },
  actions: {
    toggleFavoriteId({ commit }, mediaId: string) {
      commit('toggleFavoriteId', mediaId);
    }
  }
}