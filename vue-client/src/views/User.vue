<template>
  <ContentWrapper>
    <v-card>
      <v-card-title>User page</v-card-title>
    </v-card>

    <v-btn
      class='mt-4'
      color='secondary'
      @click='logout'>

      <v-icon>mdi-logout</v-icon>
      <span>Logout</span>
    </v-btn>
  </ContentWrapper>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import ContentWrapper from '../shared/components/content-wrapper/ContentWrapper.vue';
import store from '../store';

@Component({
  name: 'User',
  components: {
    ContentWrapper
  }
})
export default class User extends Vue {
  $auth: any;

  logout() {
    this.$auth.logout({
      returnTo: window.location.origin
    });
  }
}
</script>
