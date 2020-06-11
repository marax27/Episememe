<template>
  <v-app-bar app id="nav-bar">
    <div class="d-flex align-center">

      <NavBarLink link='/' icon='mdi-hexagon-outline' :title='true'>
        Episememe
      </NavBarLink>

      <NavBarLink link='/upload' icon='mdi-upload'>
        Upload
      </NavBarLink>

      <NavBarLink @click='openTagRelationships' icon='mdi-tag-multiple-outline'>
        Tag Relationships
      </NavBarLink>
    </div>

    <v-spacer></v-spacer>

    <AuthWidget v-if='authorizationEnabled'/>
    <NavBarLink v-else link='/user' icon='mdi-home-account'>User panel</NavBarLink>
  </v-app-bar>
</template>

<script lang='ts'>
import { Vue, Component } from 'vue-property-decorator';
import AuthWidget from './components/AuthWidget.vue';
import NavBarLink from './components/NavBarLink.vue';
import { isAuthorizationEnabled } from '../auth/helpers';

@Component({
  name: 'NavBar',
  components: {
    AuthWidget,
    NavBarLink
  }
})
export default class NavBar extends Vue {

  authorizationEnabled = true;

  created() {
    this.authorizationEnabled = isAuthorizationEnabled();
  }

  openTagRelationships() {
    this.$store.dispatch('popups/openTagRelationships');
  }
}
</script>
