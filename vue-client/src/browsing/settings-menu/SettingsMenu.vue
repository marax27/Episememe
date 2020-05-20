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

    <BaseMenuButton
      v-for='item in menuButtons' :key='item.name'
      :icon='item.icon' :label='item.name'
      @click='item.callback' />
    <VolumeMenuButton
      @click='toggleVolume' />

  </v-speed-dial>
</template>

<script lang='ts'>
import { Component, Vue } from 'vue-property-decorator';
import BaseMenuButton from './buttons/BaseMenuButton.vue';
import VolumeMenuButton from './buttons/VolumeMenuButton.vue';

@Component({
  components: {
    BaseMenuButton,
    VolumeMenuButton,
  }
})
export default class SettingsMenu extends Vue {
  menuButtons = [
    { name: 'Revise', icon: 'mdi-square-edit-outline', callback: this.startRevision },
    { name: 'Favourite', icon: 'mdi-star', callback: this.foo },
    { name: 'Hide', icon: 'mdi-eye-off', callback: this.foo },
    { name: 'Fill space', icon: 'mdi-arrow-expand', callback: this.foo },
  ];

  speedDialIsOpen = false;

  foo() {
    return undefined;
  }

  startRevision() {
    this.$emit('revise');
  }

  toggleVolume() {
    this.$store.dispatch('gallery/toggleMuted');
  }
}
</script>
