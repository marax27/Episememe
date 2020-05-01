import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    browseToken: ''
  },
  mutations: {
    setBrowseToken(state, newToken) {
      Vue.set(state, 'browseToken', newToken);
    }
  },
  actions: {
    refreshBrowseToken({ commit }, newToken) {
      commit('setBrowseToken', newToken);
    }
  }
})
