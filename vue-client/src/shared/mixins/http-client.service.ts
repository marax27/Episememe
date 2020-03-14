import axios from 'axios';
import { Vue, Component } from 'vue-property-decorator';

@Component
export default class HttpClientService extends Vue {
  $http = axios;
}
