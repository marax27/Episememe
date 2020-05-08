<template>
  <v-icon v-if="$auth.loading">mdi-loading mdi-spin</v-icon>
  <div v-else class="d-flex align-center">
    <div v-if="$auth.isAuthenticated">

      <v-menu bottom left offset-y absolute>
        <template v-slot:activator='{on}'>
          <a href='#'>
            <v-avatar class='profile-image' v-on='on'>
              <v-img
                :src="$auth.user.picture" :alt="$auth.user.name"
                transition='scale-transition'/>
            </v-avatar>
          </a>
        </template>

        <v-list>
          <v-list-item @click='goToUserPanel'>
            <v-list-item-icon>
              <v-icon>mdi-account-outline</v-icon>
            </v-list-item-icon>
            <v-list-item-content>
              User Panel
            </v-list-item-content>
          </v-list-item>

          <v-list-item @click='logout'>
            <v-list-item-icon>
              <v-icon>mdi-logout</v-icon>
            </v-list-item-icon>
            <v-list-item-content>
              Logout
            </v-list-item-content>
          </v-list-item>
        </v-list>
      </v-menu>

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

  logout() {
    this.$auth.logout({
      returnTo: window.location.origin
    });
  }

  goToUserPanel() {
    this.$router.push('/user');
  }
}
</script>
