<template>
  <BaseMenuButton
    :icon='isMuted ? "mdi-volume-off" : "mdi-volume-high"'
    :label='isMuted ? "Volume off" : "Volume on"'
    @click='$emit("click")'/>
</template>

<script lang='ts'>
import { Component, Vue } from 'vue-property-decorator';
import BaseMenuButton from './BaseMenuButton.vue';

@Component({
  components: { BaseMenuButton }
})
export default class VolumeMenuButton extends Vue {

  get isMuted(): boolean {
    return this.$store.state.gallery.isMuted;
  }
}
</script>
