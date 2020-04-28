<template>
  <v-container>
    <v-row class='text-center'>
      <v-col cols='12'>
        <slot></slot>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang='ts'>
import { Vue, Component } from 'vue-property-decorator';

@Component
export default class ContentWrapper extends Vue {}
</script>
