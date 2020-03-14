import Vue from 'vue';
import Component from 'vue-class-component';
import router from '../router';
import { useAuth0 } from '.';
import { domain, clientId } from '../../auth.config.json';

@Component
export default class AuthService extends Vue {
  $auth: any;

  created() {
    this.$auth = useAuth0({
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
  }
}
