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

  </v-speed-dial>
</template>

<script lang='ts'>
import { Component, Vue } from 'vue-property-decorator';
import BaseMenuButton from './buttons/BaseMenuButton.vue';

@Component({
  components: {
    BaseMenuButton,
  }
})
export default class SettingsMenu extends Vue {
  menuButtons = [
    { name: 'Revise', icon: 'mdi-square-edit-outline', callback: this.startRevision },
    { name: 'Favourite', icon: 'mdi-star', callback: this.foo },
    { name: 'Hide', icon: 'mdi-eye-off', callback: this.foo },
    { name: 'Volume', icon: 'mdi-volume-high', callback: this.foo },
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
