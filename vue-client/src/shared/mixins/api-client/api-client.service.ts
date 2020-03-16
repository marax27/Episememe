import { Vue, Component } from 'vue-property-decorator';
import { apiUrl } from '../../../../api.config.json';
import ApiClient from './api-client';

@Component({
  name: 'ApiClientService'
})
export default class ApiClientService extends Vue {
  $api!: ApiClient;

  $auth: any;  // it's necessary, as API calls require a token

  created() {
    this.$api = new ApiClient(this.$auth, apiUrl);
  }
}
