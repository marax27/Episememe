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

  mounted() {
    this.updateIsMuted();
  }

  @Watch('active')
  private onActiveChanged(value: boolean) {
    this.video.load();
    if (value) {
      this.video.play();
    }
  }

  private get isMuted(): boolean {
    return this.$store.state.gallery.isMuted;
  }
  @Watch('isMuted') onIsMutedChange(newValue: boolean) {
    this.updateIsMuted();
  }

  private updateIsMuted() {
    this.video.muted = this.isMuted;
  }

  private get video(): HTMLMediaElement {
    return this.$refs.videoRef as HTMLMediaElement;
  }
}
</script>
