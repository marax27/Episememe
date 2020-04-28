<template>
  <v-speed-dial
    absolute
    v-model='speedDialIsOpen'
    :right='true' :bottom='true'
    direction='top' transition='slide-y-reverse-transition'
    :open-on-hover='true'>

    <template v-slot:activator>
      <v-btn v-model='speedDialIsOpen' color='secondary' fab>
        <v-icon>mdi-menu</v-icon>
      </v-btn>
    </template>

    <v-tooltip left v-for='item in menuButtons' :key='item.name'>
      <template v-slot:activator='{ on }'>
        <v-btn @click='item.callback'
          v-on='on' fab small color='secondary' @click.stop>
          <v-icon>{{ item.icon }}</v-icon>
        </v-btn>
      </template>
      <span>{{ item.name }}</span>
    </v-tooltip>

  </v-speed-dial>
</template>

<script lang='ts'>
import { Component, Vue } from 'vue-property-decorator';

@Component
export default class SettingsMenu extends Vue {
  menuButtons = [
    { name: 'Revise', icon: 'mdi-pencil', callback: this.foo },
    { name: 'Favourite', icon: 'mdi-star', callback: this.foo },
    { name: 'Hide', icon: 'mdi-eye-off', callback: this.foo },
    { name: 'Volume', icon: 'mdi-volume-high', callback: this.foo },
    { name: 'Fill space', icon: 'mdi-arrow-expand', callback: this.foo },
  ];

  foo() {
    console.log('foo');
  }
}
</script>

<style scoped>

</style>
