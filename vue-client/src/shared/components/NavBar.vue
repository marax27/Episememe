<template>
  <nav>
    <div class="left">
      <router-link to="/">Home</router-link> | 
      <router-link to="/about">About</router-link>
    </div>
    <div class="right" style='height: 100%'>
      <AuthWidget/>
    </div>
    <div style='clear: both'></div>
  </nav>
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
nav {
  padding: 30px;
  border-bottom: solid 1px #42b983;
}

nav a {
  font-weight: bold;
  color: #2c3e50;
}

nav a.router-link-exact-active {
  color: #42b983;
}
</style>