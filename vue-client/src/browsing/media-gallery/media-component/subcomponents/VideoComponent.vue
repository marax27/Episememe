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
    this.updateAutoloop();
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
  @Watch('isMuted') onIsMutedChange() {
    this.updateIsMuted();
  }

  private updateIsMuted() {
    this.video.muted = this.isMuted;
  }

  private get autoloop(): boolean {
    return this.$store.state.gallery.autoloop;
  }
  @Watch('autoloop') onAutoloopChange() {
    this.updateAutoloop();
  }

  private updateAutoloop() {
    this.video.loop = this.autoloop;
  }

  private get video(): HTMLMediaElement {
    return this.$refs.videoRef as HTMLMediaElement;
  }
}
</script>
