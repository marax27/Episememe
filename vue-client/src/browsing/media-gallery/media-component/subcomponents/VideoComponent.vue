<template>
  <video ref='videoRef' controls>
    <source :src='url' :type='"video/" + type'/>
    {{ alt }}
  </video>
</template>

<script lang='ts'>
import { Component, Prop, Vue, Watch } from 'vue-property-decorator';
import { IHasResolution } from '../../../interfaces/IHasResolution';
import { ResolutionModes } from '../../../types/ResolutionModes';

@Component
export default class VideoComponent extends Vue implements IHasResolution {
  @Prop()
  url?: string;

  @Prop()
  type?: string;

  @Prop()
  alt?: string;

  @Prop()
  active?: boolean;

  private resolutionMode: ResolutionModes = ResolutionModes.Unknown;

  mounted() {
    this.updateIsMuted();
    this.updateAutoloop();
  }

  public getResolution(): Promise<ResolutionModes> {
    const video = this.video;
    if (this.resolutionMode !== ResolutionModes.Unknown) {
      // Resolution has been requested before.
      return Promise.resolve(this.resolutionMode);
    } else if (video.videoWidth !== 0 && video.videoWidth != undefined) {
      // Video metadata loaded, but this is the 1st time resolution is requested.
      this.updateResolutionMode(video.videoWidth, video.videoHeight);
      return Promise.resolve(this.resolutionMode);
    } else {
      // Video metadata not yet loaded.
      return new Promise((resolve, reject) => {
        video.addEventListener('loadedmetadata', () => {
          this.updateResolutionMode(video.videoWidth, video.videoHeight);
          resolve(this.resolutionMode);
        });
      });
    }
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

  private get video(): HTMLVideoElement {
    return this.$refs.videoRef as HTMLVideoElement;
  }

  private updateResolutionMode(width: number, height: number) {
    let value: ResolutionModes;
    if (width > height)
      value = ResolutionModes.Landscape;
    else if (height > width)
      value = ResolutionModes.Portrait;
    else
      value = ResolutionModes.Unknown;
    this.resolutionMode = value;
  }
}
</script>
