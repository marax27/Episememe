import { Module } from 'vuex';

interface IPopup {
  isOpen: boolean;
}

export const popups: Module<any, any> = {
  namespaced: true,
  state: () => ({
    tagRelationships: {
      isOpen: false
    },
    revision: {
      isOpen: false
    }
  }),
  actions: {
    openTagRelationships({ commit }) {
      commit('setTagRelationships', { isOpen: true });
    },
    closeTagRelationships({ commit }) {
      commit('setTagRelationships', { isOpen: false });
    },
    openRevision({ commit }) {
      commit('setRevision', { isOpen: true });
    },
    closeRevision({ commit }) {
      commit('setRevision', { isOpen: false });
    }
  },
  mutations: {
    setTagRelationships(state, newValue: IPopup) {
      state.tagRelationships = newValue;
    },
    setRevision(state, newValue: IPopup) {
      state.revision = newValue;
    }
  }
};
