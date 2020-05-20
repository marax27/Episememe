<template>
  <video ref='videoRef' controls>
    <source :src='url' :type='"video/" + type'/>
    {{ alt }}
  </video>
</template>

<script lang='ts'>
import { Component, Prop, Vue, Watch } from 'vue-property-decorator';

@Component
export default class VideoComponent extends Vue {
  @Prop()
  url?: string;

  @Prop()
  type?: string;

  @Prop()
  alt?: string;

  @Prop()
  active?: boolean;

  @Watch('active')
  private onActiveChanged(value: boolean) {
    const el = this.videoElement;
    el.load();
    if (value) {
      el.play();
    }
  }

  private get isMuted() {
    return this.$store.state.gallery.isMuted;
  }
  @Watch('isMuted')
  onIsMutedChange(newValue: boolean) {
    this.videoElement.muted = newValue;
  }

  private get videoElement() {
    return this.$refs.videoRef as HTMLMediaElement;
  }
}
</script>
