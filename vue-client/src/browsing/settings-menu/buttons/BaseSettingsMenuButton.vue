<template>
  <v-tooltip left :key='label'>
    <template v-slot:activator='{ on }'>
      <v-btn @click='$emit("click", $event)'
        v-on='on' fab small color='secondary' @click.stop>
        <v-icon :color='color'>{{ icon }}</v-icon>
      </v-btn>
    </template>
    <span>{{ label }}</span>
  </v-tooltip>
</template>

<script lang='ts'>
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component
export default class BaseSettingsMenuButton extends Vue {
  @Prop()
  icon?: string;

  @Prop()
  label?: string;

  @Prop()
  color?: string;
}
</script>
