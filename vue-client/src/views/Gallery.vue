<template>
  <div class='gallery-wrapper'>

    <v-container class='gallery pa-0' fluid fill-height>
      <img class='media-instance' src='@/assets/logo.png' alt='Image'/>
    </v-container>

    <v-btn class='previous-instance' x-large icon color='secondary'>
      <v-icon>mdi-arrow-left</v-icon>
    </v-btn>

    <v-btn class='next-instance' x-large icon color='secondary'>
      <v-icon>mdi-arrow-right</v-icon>
    </v-btn>

    <v-speed-dial absolute
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
            v-on='on' fab small color='secondary'>
            <v-icon>{{ item.icon }}</v-icon>
          </v-btn>
        </template>
        <span>{{ item.name }}</span>
      </v-tooltip>

    </v-speed-dial>
  </div>
</template>

<script lang='ts'>
import { Component, Prop, Vue } from 'vue-property-decorator';

@Component
export default class Gallery extends Vue {

  speedDialIsOpen = false;

  menuButtons = [
    { name: 'Revise', icon: 'mdi-pencil', callback: this.foo1 },
    { name: 'Favourite', icon: 'mdi-star', callback: this.foo2 },
    { name: 'Hide', icon: 'mdi-eye-off', callback: this.foo3 },
    { name: 'Volume', icon: 'mdi-volume-high', callback: this.foo4 },
    { name: 'Fill space', icon: 'mdi-arrow-expand', callback: this.foo5 },
  ];

  public get galleryData() {
    return this.$route.params.data;
  }

  foo1() { console.log('Foo1'); }
  foo2() { console.log('Foo2'); }
  foo3() { console.log('Foo3'); }
  foo4() { console.log('Foo4'); }
  foo5() { console.log('Foo5'); }
}
</script>

<style scoped>
.gallery-wrapper {
  width: 100%;
  height: 100%;
}

.gallery {
  background-color: black;
}

.gallery .media-instance {
  display: block;
  margin: 0 auto;
}

.previous-instance, .next-instance {
  position: fixed;
  top: 50%;
}
.previous-instance { left: 0; }
.next-instance { right: 0; }

.previous-instance:hover, .next-instance:hover {
  color: white !important;
}
</style>
