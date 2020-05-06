<template>
  <v-icon v-if="$auth.loading">mdi-loading mdi-spin</v-icon>
  <div v-else class="d-flex align-center">
    <div v-if="$auth.isAuthenticated">
      <router-link to="/user">
        <v-avatar class="profile-image">
          <v-img
            :src="$auth.user.picture" :alt="$auth.user.name"
            transition="scale-transition"/>
        </v-avatar>
      </router-link>
    </div>
    <v-btn v-else text @click="login">
      <v-icon>mdi-login</v-icon>
      <span>Login</span>
    </v-btn>
  </div>
</template>

<script lang='ts'>
import { Vue, Component } from 'vue-property-decorator';
import store from '../../store';

@Component({
  name: 'AuthWidget'
})
export default class AuthWidget extends Vue {
  $auth: any;

  login() {
    this.$auth.loginWithRedirect();
  }
}
</script>
