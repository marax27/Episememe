<template>
  <v-tooltip left :key='label'>
    <template v-slot:activator='{ on }'>
      <v-btn
        v-on='on'
        @click.stop
        @click='$emit("click")'
        fab small color='secondary'>

        <v-icon>{{ icon }}</v-icon>
      </v-btn>
    </template>
    <span>{{ label }}</span>
  </v-tooltip>
</template>

<script lang='ts'>
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component
export default class BaseMenuButton extends Vue {
  @Prop({ default: '' })
  icon!: string;

  @Prop({ default: '' })
  label!: string;
}
</script>
