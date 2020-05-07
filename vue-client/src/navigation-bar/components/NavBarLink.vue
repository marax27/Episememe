<template>
  <router-link
    :to='link'
    class='nav-link white--text'
    v-bind:class='{ "title": title }'>

    <v-icon v-if='!!icon'
            :left='!isSmall'
            :large='!title && isSmall'
            :x-large='title'>
      {{ icon }}
    </v-icon>

    <span v-show='!isSmall'>
      <slot></slot>
    </span>

  </router-link>
</template>

<script lang='ts'>
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component
export default class NavBarLink extends Vue {
  @Prop()
  link!: string;

  @Prop({ default: '' })
  icon!: string;

  @Prop({ default: false })
  title!: boolean;

  get isSmall(): boolean {
    return this.$vuetify.breakpoint.xsOnly;
  }
}
</script>

<style scoped>
.nav-link {
  text-decoration: none;
  border-bottom: solid 1px;
  border-bottom-color: transparent;
  display: flex;
  align-items: center;
}

.nav-link:hover {
  border-bottom-color: rgba(256, 256, 256, 0.7);
}
.nav-link:active {
  border-bottom-color: rgba(256, 256, 256, 1);
}

.nav-link + .nav-link {
  margin-left: 1.5em;
  margin-right: .5em;
}

a:active, a:focus {
  outline: 0;
}
</style>
