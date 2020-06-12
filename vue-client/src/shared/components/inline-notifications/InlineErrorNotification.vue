<template>
  <p>
    <v-icon left color='error'>mdi-alert-circle-outline</v-icon>
    <span class='error--text'>{{ message }}</span>
  </p>
</template>

<script lang='ts'>
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component
export default class InlineErrorNotification extends Vue {
  @Prop()
  message?: string;
}
</script>
