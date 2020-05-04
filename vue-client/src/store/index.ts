import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    browseToken: '',
    tags: null
  },
  mutations: {
    setBrowseToken(state, newToken) {
      Vue.set(state, 'browseToken', newToken);
    },
    setTags(state, newTags) {
      Vue.set(state, 'tags', newTags);
    }
  },
  actions: {
    refreshBrowseToken({ commit }, newToken) {
      commit('setBrowseToken', newToken);
    },
    updateTags({ commit }, tags) {
      commit('setTags', tags);
    }
  },
  getters: {
    allTags: state => {
      return state.tags ?? [];
    }
  }
})
