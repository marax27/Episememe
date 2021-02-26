<template>
  <video ref='videoRef' controls>
    <source :src='url' :type='"video/" + type'/>
    {{ alt }}
  </video>
</template>

<script lang='ts'>
import { Component, Prop, Vue, Watch } from 'vue-property-decorator';
import { IHasResolution } from '../../../interfaces/IHasResolution';
import { IResolution } from '../../../interfaces/IResolution';

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

  private resolution: IResolution | null = null;

  mounted() {
    this.updateIsMuted();
    this.updateAutoloop();
  }

  public getResolution(): Promise<IResolution> {
    const video = this.video;
    if (this.resolution != null) {
      // Resolution has been requested before.
      return Promise.resolve(this.resolution as IResolution);
    } else if (video.videoWidth !== 0 && video.videoWidth != undefined) {
      // Video metadata loaded, but this is the 1st time resolution is requested.
      const result = this.updateResolution(video.videoWidth, video.videoHeight);
      return Promise.resolve(result);
    } else {
      // Video metadata not yet loaded.
      return new Promise((resolve, reject) => {
        video.addEventListener('loadedmetadata', () => {
          const result = this.updateResolution(video.videoWidth, video.videoHeight);
          resolve(result);
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

  private updateResolution(width: number, height: number): IResolution {
    const result = { width: width, height: height };
    this.resolution = result;
    return result;
  }
}
</script>
