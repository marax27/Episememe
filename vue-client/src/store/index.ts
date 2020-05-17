import Vue from 'vue';
import Vuex from 'vuex';
import { gallery } from './modules/gallery.module';

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    browseToken: '',
    tags: null,
    errorMessage: null
  },
  mutations: {
    setBrowseToken(state, newToken) {
      Vue.set(state, 'browseToken', newToken);
    },
    setTags(state, newTags) {
      Vue.set(state, 'tags', newTags);
    },
    setErrorMessage(state, newMessage) {
      Vue.set(state, 'errorMessage', newMessage);
    }
  },
  actions: {
    refreshBrowseToken({ commit }, newToken) {
      commit('setBrowseToken', newToken);
    },
    updateTags({ commit }, tags) {
      commit('setTags', tags);
    },
    reportError({ commit }, message) {
      commit('setErrorMessage', message);
    },
    clearError({ commit }) {
      commit('setErrorMessage', null)
    }
  },
  getters: {
    allTags: state => {
      return state.tags ?? [];
    }
  },
  modules: {
    gallery
  }
})
