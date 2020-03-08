import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'

import { domain, clientId } from '../auth.config.json'
import { Auth0Plugin } from './auth'

Vue.use(Auth0Plugin, {
  domain,
  clientId,
  onRedirectCallback: (appState: any) => {
    router.push(
      appState && appState.targetUrl
        ? appState.targetUrl
        : window.location.pathname
    );
  }
});

Vue.config.productionTip = false

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
