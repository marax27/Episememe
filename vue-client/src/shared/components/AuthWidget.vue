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

<script>
export default {
  name: 'AuthWidget',
  components: {},
  methods: {
    login() {
      this.$auth.loginWithRedirect();
    },
    logout() {
      this.$auth.logout({
        returnTo: window.location.origin
      });
    }
  }
}
</script>

<style>

</style>
