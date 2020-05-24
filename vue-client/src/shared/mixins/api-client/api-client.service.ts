import { Vue, Component } from 'vue-property-decorator';
import ApiClient from './api-client';
import { isAuthorizationEnabled } from '@/auth/helpers';

@Component({
  name: 'ApiClientService'
})
export default class ApiClientService extends Vue {
  $api!: ApiClient;

  $auth: any;  // it's necessary, as API calls require a token

  created() {
    const authComponent = isAuthorizationEnabled() ? this.$auth : null;
    this.$api = new ApiClient(authComponent, process.env.VUE_APP_API_URL);
  }
}
