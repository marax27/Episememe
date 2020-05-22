<template>
  <v-hover
    v-slot:default='{ hover }'>

    <v-card
      ripple
      @click='$emit("click")'
      class='tile'
      raised
      :color='hover ? "secondary" : undefined'>

      <v-card-title>
        <v-icon left>mdi-cursor-default-click</v-icon>
        Deduce tags
      </v-card-title>

      <v-card-text>
        <p>Treat filename as a list of space-separated tag names. Use hyphens to provide multi-word tags, e.g.:</p>
        <p class='font-weight-bold'>"tag another-tag last-tag.xyz"</p>
      </v-card-text>
    </v-card>
  </v-hover>
</template>

<script lang='ts'>
import { Component, Vue } from 'vue-property-decorator';

@Component
export default class DeduceTagsTile extends Vue {}
</script>

<style scoped>
.tile {
  user-select: none;
}
</style>
