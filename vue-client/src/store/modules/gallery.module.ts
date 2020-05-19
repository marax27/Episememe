import { Module } from 'vuex';
import { IMediaInstance } from '@/shared/models/IMediaInstance';

export const gallery: Module<any, any> = {
  namespaced: true,
  state: () => ({
    currentMediaInstance: null
  }),
  mutations: {
    setCurrentMediaInstance(state, newInstance: IMediaInstance) {
      state.currentMediaInstance = newInstance;
    }
  },
  actions: {
    updateCurrentMediaInstance({ commit }, instance: IMediaInstance) {
      commit('setCurrentMediaInstance', instance);
    }
  },
  getters: {
    
  }
};
