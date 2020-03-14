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
import { Vue, Component } from 'vue-property-decorator';

@Component({
  name: 'AuthWidget'
})
export default class AuthWidget extends Vue {
  $auth: any;

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
