<template>
  <v-icon v-if="$auth.loading">mdi-loading mdi-spin</v-icon>
  <div v-else>
    <div v-if="$auth.isAuthenticated">
      <v-btn text @click="logout">
        <v-icon>mdi-logout</v-icon>
        <span>Logout</span>
      </v-btn>

      <a href="#">
      <v-avatar class="profile-image">
        <v-img
          :src="$auth.user.picture" :alt="$auth.user.name"
          transition="scale-transition"/>
      </v-avatar>
      </a>
    </div>
    <v-btn v-else text @click="login">
      <v-icon>mdi-login</v-icon>
      <span>Login</span>
    </v-btn>
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
