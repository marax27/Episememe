import { Module } from 'vuex';
import { IMediaInstance } from '@/shared/models/IMediaInstance';

export const gallery: Module<any, any> = {
  namespaced: true,
  state: () => ({
    currentMediaInstance: null,
    isMuted: false,
  }),
  mutations: {
    setCurrentMediaInstance(state, newInstance: IMediaInstance) {
      state.currentMediaInstance = newInstance;
    },
    setMuted(state, isMuted: boolean) {
      state.isMuted = isMuted;
    }
  },
  actions: {
    updateCurrentMediaInstance({ commit }, instance: IMediaInstance) {
      commit('setCurrentMediaInstance', instance);
    },
    toggleMuted({ commit, state }) {
      commit('setMuted', !state.isMuted);
    }
  },
  getters: {
    
  }
};
