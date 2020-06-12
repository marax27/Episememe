<template>
  <p>
    <v-icon left color='success'>mdi-hand-okay</v-icon>
    <span class='success--text'>{{ message }}</span>
  </p>
</template>

<script lang='ts'>
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component
export default class InlineSuccessNotification extends Vue {
  @Prop()
  message?: string;
}
</script>
