import { Module } from 'vuex';
import { IMediaInstance } from '@/shared/models/IMediaInstance';

export const gallery: Module<any, any> = {
  namespaced: true,
  state: () => ({
    currentMediaInstance: null,
    isMuted: false,
    autoloop: true,
  }),
  mutations: {
    setCurrentMediaInstance(state, newInstance: IMediaInstance) {
      state.currentMediaInstance = newInstance;
    },
    setIsMuted(state, isMuted: boolean) {
      state.isMuted = isMuted;
    },
    setAutoloop(state, newValue: boolean) {
      state.autoloop = newValue;
    }
  },
  actions: {
    updateCurrentMediaInstance({ commit }, instance: IMediaInstance) {
      commit('setCurrentMediaInstance', instance);
    },
    toggleVolume({ commit, state }) {
      commit('setIsMuted', !state.isMuted);
    },
    toggleAutoloop({ commit, state }) {
      commit('setAutoloop', !state.autoloop);
    }
  },
  getters: {
    
  }
};
