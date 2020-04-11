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

      <router-link to="/" class="title nav-link white--text">Episememe</router-link>

      <router-link to="/about" class="nav-link white--text">About</router-link>
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

<style scoped>
.nav-link {
  text-decoration: none;
  border-bottom: solid 1px;
  border-bottom-color: transparent;
}
.nav-link:hover {
  border-bottom-color: rgba(256, 256, 256, 0.7);
}
.nav-link:active {
  border-bottom-color: rgba(256, 256, 256, 1);
}

.nav-link + .nav-link {
  margin-left: 10px;
}
</style>
