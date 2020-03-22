<template>
  <v-app-bar app color="primary" dark id="nav-bar">
    <div class="d-flex align-center">
      <v-img
        alt="Vuetify Logo"
        class="shrink mr-2"
        contain
        src="https://cdn.vuetifyjs.com/images/logos/vuetify-logo-dark.png"
        transition="scale-transition"
        width="40"/>

      <v-btn to="/" class="title" text>
        <span>Episememe</span>
      </v-btn>

      <v-btn to="/about" text>
        <span>About</span>
      </v-btn>
    </div>

    <v-spacer></v-spacer>

    <AuthWidget/>
  </v-app-bar>
</template>

<script lang='ts'>
import { Vue, Component } from 'vue-property-decorator';
import AuthWidget from './AuthWidget.vue';

@Component({
  name: 'NavBar',
  components: {
    AuthWidget
  }
})
export default class NavBar extends Vue {}
</script>
