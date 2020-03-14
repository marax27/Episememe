<template>
  <span v-if="$auth.loading">
    Loading...
  </span>
  <div v-else>
    <div v-if="$auth.isAuthenticated">
      <button @click="logout">Log out</button>
    </div>
    <button v-else @click="login">Log in</button>
  </div>
</template>

<script lang='ts'>
import { Component, Mixins, Vue } from 'vue-property-decorator';
import AuthService from '../../auth/auth.service';

@Component({
  name: 'AuthWidget'
})
export default class AuthWidget extends Mixins(AuthService) {
  login() {
    this.$auth.loginWithRedirect();
  }

  logout() {
    this.$auth.logout({
      returnTo: window.location.origin
    });
  }
}
</script>
