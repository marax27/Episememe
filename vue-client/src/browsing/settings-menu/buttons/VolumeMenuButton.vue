<template>
  <BaseSettingsMenuButton
    :label='isMuted ? "Volume off" : "Volume on"'
    :icon='isMuted ? "mdi-volume-off" : "mdi-volume-high"'
    @click='$emit("click", $event)' />
</template>

<script lang='ts'>
import { Component, Vue } from 'vue-property-decorator';
import BaseSettingsMenuButton from './BaseSettingsMenuButton.vue';

@Component({
  components: {
    BaseSettingsMenuButton
  }
})
export default class VolumeMenuButton extends Vue {

  get isMuted(): boolean {
    return this.$store.state.gallery.isMuted;
  }
}
</script>
