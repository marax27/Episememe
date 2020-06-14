<template>
  <v-speed-dial
    absolute
    v-model='speedDialIsOpen'
    :right='true' :bottom='true'
    :open-on-hover='true'>

    <template v-slot:activator>
      <v-btn v-model='speedDialIsOpen' color='secondary' fab>
        <v-icon>mdi-menu</v-icon>
      </v-btn>
    </template>

    <RevisionMenuButton @click='startRevision' />
    <VolumeMenuButton @click='toggleVolume' />
    <AutoloopMenuButton @click='toggleAutoloop' />
    <FavoriteMenuButton />

  </v-speed-dial>
</template>

<script lang='ts'>
import { Component, Vue } from 'vue-property-decorator';
import BaseSettingsMenuButton from './buttons/BaseSettingsMenuButton.vue';
import RevisionMenuButton from './buttons/RevisionMenuButton.vue';
import VolumeMenuButton from './buttons/VolumeMenuButton.vue';
import AutoloopMenuButton from './buttons/AutoloopMenuButton.vue';
import FavoriteMenuButton from './buttons/FavoriteMenuButton.vue';

@Component({
  components: {
    BaseSettingsMenuButton,
    RevisionMenuButton,
    VolumeMenuButton,
    AutoloopMenuButton,
    FavoriteMenuButton,
  }
})
export default class SettingsMenu extends Vue {
  speedDialIsOpen = false;

  foo() {
    return undefined;
  }

  startRevision() {
    this.$store.dispatch('popups/openRevision');
  }

  toggleVolume() {
    this.$store.dispatch('gallery/toggleVolume');
  }

  toggleAutoloop() {
    this.$store.dispatch('gallery/toggleAutoloop');
  }
}
</script>
