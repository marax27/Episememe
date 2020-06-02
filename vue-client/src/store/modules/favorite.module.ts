import { Module } from 'vuex';

export const favorite: Module<any, any> = {
  namespaced: true,
  state: () => ({
    favoriteIds: null,
  }),
  mutations: {
    toggleFavoriteId(state, mediaId: string) {
      const index = state.favoriteIds.indexOf(mediaId);
      if (index === -1) {
        state.favoriteIds.push(mediaId);
      } else {
        state.favoriteIds.splice(index, 1);
      }
    },
    setFavoriteIds(state, mediaIds: string[]) {
      state.favoriteIds = mediaIds;
    }
  },
  actions: {
    toggleFavoriteId({ commit }, mediaId: string) {
      commit('toggleFavoriteId', mediaId);
    },
    updateFavoriteIds({ commit }, mediaIds: string[]) {
      commit('setFavoriteIds', mediaIds);
    }
  }
}