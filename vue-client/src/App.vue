<template>
  <v-app>
    <NavBar/>
    <v-content>
      <router-view/>
    </v-content>
    <ErrorNotification/>
  </v-app>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';
import NavBar from './navigation-bar/NavBar.vue';
import ErrorNotification from './shared/components/error-notification/ErrorNotification.vue';

@Component({
  name: 'App',
  components: {
    NavBar,
    ErrorNotification
  }
})
export default class App extends Vue {}
</script>
