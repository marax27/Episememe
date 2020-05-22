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
    <BaseSettingsMenuButton
      v-for='item in menuButtons' :key='item.name'
      :label='item.name' :icon='item.icon' />

  </v-speed-dial>
</template>

<script lang='ts'>
import { Component, Vue } from 'vue-property-decorator';
import BaseSettingsMenuButton from './buttons/BaseSettingsMenuButton.vue';
import RevisionMenuButton from './buttons/RevisionMenuButton.vue';

@Component({
  components: {
    BaseSettingsMenuButton,
    RevisionMenuButton,
  }
})
export default class SettingsMenu extends Vue {
  menuButtons = [
    { name: 'Favourite', icon: 'mdi-star', callback: this.foo },
    { name: 'Hide', icon: 'mdi-eye-off', callback: this.foo },
    { name: 'Volume', icon: 'mdi-volume-high', callback: this.foo },
    { name: 'Loop', icon: 'mdi-restart', callback: this.foo },
    { name: 'Fill space', icon: 'mdi-arrow-expand', callback: this.foo },
  ];

  speedDialIsOpen = false;

  foo() {
    return undefined;
  }

  startRevision() {
    this.$emit('revise');
  }
}
</script>
