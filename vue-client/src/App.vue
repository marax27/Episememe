<template>
  <v-app>
    <NavBar/>
    <v-content>
      <router-view/>
    </v-content>
  </v-app>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';
import NavBar from './navigation-bar/NavBar.vue';

@Component({
  name: 'App',
  components: {
    NavBar
  }
})
export default class App extends Vue {}
</script>
