<template>
  <v-app>
    <NavBar/>
    <v-content>
      <router-view/>
    </v-content>
    <ErrorNotification/>
    <TagRelationshipsPopup/>
  </v-app>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';
import NavBar from './navigation-bar/NavBar.vue';
import ErrorNotification from './shared/components/error-notification/ErrorNotification.vue';
import TagRelationshipsPopup from './tags/components/TagRelationshipsPopup.vue';

@Component({
  name: 'App',
  components: {
    NavBar,
    TagRelationshipsPopup,
    ErrorNotification
  }
})
export default class App extends Vue {}
</script>
