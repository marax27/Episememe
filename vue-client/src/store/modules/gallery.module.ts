import { Module } from 'vuex';
import { IMediaInstance } from '@/shared/models/IMediaInstance';
import { LayoutModes } from '@/browsing/types/LayoutModes';

export const gallery: Module<any, any> = {
  namespaced: true,
  state: () => ({
    currentMediaInstance: null,
    isMuted: false,
    autoloop: true,
    layoutMode: LayoutModes.OriginalOrFit,
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
    },
    setLayoutMode(state, newMode: LayoutModes) {
      state.layoutMode = newMode;
    },
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
    },
    toggleLayoutMode({ commit, state }) {
      const map: { [key in LayoutModes]: LayoutModes } = {
        [LayoutModes.OriginalOrFit]: LayoutModes.AspectFit,
        [LayoutModes.AspectFit]: LayoutModes.AspectFill,
        [LayoutModes.AspectFill]: LayoutModes.OriginalOrFit,
      };

      const newValue = map[state.layoutMode as LayoutModes];
      commit('setLayoutMode', newValue);
    },
  },
  getters: {
    currentMediaId: state => {
      return state.currentMediaInstance.id;
    }
  }
};
