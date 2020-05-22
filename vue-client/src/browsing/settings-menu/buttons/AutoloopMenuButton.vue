<template>
  <BaseSettingsMenuButton
    :label="isLooping ? 'Video loop' : 'Video loopn\'t'"
    :icon="isLooping ? 'mdi-restart' : 'mdi-restart-off'"
    @click="$emit('click', $event)" />
</template>

<script lang='ts'>
import { Component, Vue } from 'vue-property-decorator';
import BaseSettingsMenuButton from './BaseSettingsMenuButton.vue';

@Component({
  components: {
    BaseSettingsMenuButton
  }
})
export default class AutoloopMenuButton extends Vue {

  get isLooping(): boolean {
      return this.$store.state.gallery.autoloop;
  }
}
</script>
