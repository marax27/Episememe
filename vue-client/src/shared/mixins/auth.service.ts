import Vue from 'vue';
import Component from 'vue-class-component';

@Component
export default class AuthService extends Vue {
  getAuth(): any {
      return (this as any).$auth;
  }
}