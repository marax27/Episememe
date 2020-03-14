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
import Vue from 'vue';
import AuthWidget from './AuthWidget.vue';

export default Vue.extend({
  name: 'NavBar',
  components: {
    AuthWidget
  }
});
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