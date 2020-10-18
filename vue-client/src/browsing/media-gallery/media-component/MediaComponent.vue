<template>
  <ImageComponent v-if='getComponentType() === "image"'
    v-show='active'
    :url='mediaUrl'
    :alt='altMessage'
    ref='imageRef'>
  </ImageComponent>

  <VideoComponent v-else-if='getComponentType() === "video"'
    v-show='active'
    :url='mediaUrl'
    :type='instance.dataType'
    :alt='altMessage'
    :active='active'
    ref='videoRef'>
  </VideoComponent>

  <DownloadLinkComponent v-else
    v-show='active'
    :url='mediaUrl'
    :identifier='instance.id'>
  </DownloadLinkComponent>
</template>

<script lang='ts'>
import { Component, Prop, Mixins, Watch } from 'vue-property-decorator';
import { IMediaInstance } from '../../../shared/models/IMediaInstance';
import ImageComponent from './subcomponents/ImageComponent.vue';
import VideoComponent from './subcomponents/VideoComponent.vue';
import DownloadLinkComponent from './subcomponents/DownloadLinkComponent.vue';
import ApiClientService from '../../../shared/mixins/api-client/api-client.service';
import { ResolutionModes } from '../../types/ResolutionModes';
import { IHasResolution } from '../../interfaces/IHasResolution';
import { IResolution } from '../../interfaces/IResolution';

@Component({
  components: {
    ImageComponent,
    VideoComponent,
    DownloadLinkComponent
  }
})
export default class MediaComponent extends Mixins(ApiClientService) {
  @Prop()
  instance?: IMediaInstance;

  @Prop({ default: true })
  active!: boolean;

  mounted() {
    if (this.active)
      this.updateResolution();
  }

  public get mediaUrl(): string {
    const browseToken = this.$store.state.browseToken;
    return this.$api.createUrl(`files/${this.instance?.id}`, {token: browseToken});
  }

  public get altMessage(): string {
    return `Cannot display #${this.instance?.id}`;
  }

  public getComponentType(): string {
    const dataType = this.instance?.dataType.toLowerCase() ?? '';
    if (['jpg', 'png', 'bmp', 'svg', 'jpeg', 'gif', 'webp'].includes(dataType))
      return 'image';
    else if (['mp4', 'ogg', 'avi', 'mpeg', 'webm'].includes(dataType))
      return 'video';
    else
      return 'download';
  }

  @Watch('active')
  onActiveChanged(active: boolean) {
    if (active)
      this.updateResolution();
  }

  private updateResolution() {
    let resolutionProvider: IHasResolution;

    if (this.getComponentType() === 'image') {
      const img = this.$refs.imageRef as ImageComponent;
      resolutionProvider = img;
    } else if (this.getComponentType() === 'video') {
      const video = this.$refs.videoRef as VideoComponent;
      resolutionProvider = video;
    } else {
      this.$emit('resolution', ResolutionModes.Unknown);
      return;
    }

    resolutionProvider.getResolution().then(response =>
      this.$emit('resolution', this.computeResolutionMode(response))
    );
  }

  private computeResolutionMode(resolution: IResolution): ResolutionModes {
    const res = resolution;
    if (res.width > res.height)
      return ResolutionModes.Landscape;
    else if (res.height > res.width)
      return ResolutionModes.Portrait;
    else
      return ResolutionModes.Unknown;
  }
}
</script>
