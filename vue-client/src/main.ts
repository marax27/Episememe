import Vue from 'vue';
import App from './App.vue';
import router from './router';
import store from './store';

import { domain, clientId, audience } from '../auth.config.json'
import { Auth0Plugin } from './auth'
import vuetify from './plugins/vuetify';

require('./assets/reset.css');
require('./assets/global.css');

Vue.use(Auth0Plugin, {
  domain,
  clientId,
  audience,
  onRedirectCallback: (appState: any) => {
    router.push(
      appState && appState.targetUrl
        ? appState.targetUrl
        : window.location.pathname
    );
  }
});

Vue.config.productionTip = false;

new Vue({
  router,
  store,
  vuetify,
  render: h => h(App)
}).$mount('#app');
