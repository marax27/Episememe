<template>
  <v-card
    :disabled='disabled'
    tile
    class='align-self-stretch'
    color='secondary darken-1'>

    <v-card-title v-if='title'>{{ title }}</v-card-title>
    <v-card-text>
      <slot></slot>
    </v-card-text>
  </v-card>
</template>

<script lang='ts'>
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component
export default class  extends Vue {
  @Prop({ default: false })
  disabled!: boolean;

  @Prop({ default: '' })
  title!: string;
}
</script>
