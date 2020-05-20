import { Module } from 'vuex';
import { IMediaInstance } from '@/shared/models/IMediaInstance';

export const gallery: Module<any, any> = {
  namespaced: true,
  state: () => ({
    currentMediaInstance: null,
    volumeOn: true,
  }),
  mutations: {
    setCurrentMediaInstance(state, newInstance: IMediaInstance) {
      state.currentMediaInstance = newInstance;
    },
    setVolumeOn(state, volumeOn: boolean) {
      state.volumeOn = volumeOn;
    }
  },
  actions: {
    updateCurrentMediaInstance({ commit }, instance: IMediaInstance) {
      commit('setCurrentMediaInstance', instance);
    },
    toggleVolume({ commit, state }) {
      commit('setVolumeOn', !state.volumeOn);
    }
  },
  getters: {
    
  }
};
