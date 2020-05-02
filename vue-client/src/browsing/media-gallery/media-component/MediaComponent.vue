<template>
  <ImageComponent v-if='getComponentType() === "image"'
    v-show='active'
    :url='mediaUrl'
    :alt='altMessage'>
  </ImageComponent>

  <VideoComponent v-else-if='getComponentType() === "video"'
    v-show='active'
    :url='mediaUrl'
    :type='instance.dataType'
    :alt='altMessage'
    :active='active'>
  </VideoComponent>

  <DownloadLinkComponent v-else
    v-show='active'
    :url='mediaUrl'
    :identifier='instance.id'>
  </DownloadLinkComponent>
</template>

<script lang='ts'>
import { Component, Prop, Vue } from 'vue-property-decorator';
import { IMediaInstance } from '../../../shared/models/IMediaInstance';
import ImageComponent from './subcomponents/ImageComponent.vue';
import VideoComponent from './subcomponents/VideoComponent.vue';
import DownloadLinkComponent from './subcomponents/DownloadLinkComponent.vue';

@Component({
  components: {
    ImageComponent,
    VideoComponent,
    DownloadLinkComponent
  }
})
export default class MediaComponent extends Vue {
  @Prop()
  instance?: IMediaInstance;

  @Prop({ default: true })
  active!: boolean;

  public get mediaUrl(): string {
    return 'http://localhost:18888/' + this.instance?.id;
  }

  public get altMessage(): string {
    return `Cannot display #${this.instance?.id}`;
  }

  public getComponentType(): string {
    const dataType = this.instance?.dataType.toLowerCase() ?? '';
    if (['jpg', 'png', 'bmp', 'svg', 'jpeg'].includes(dataType))
      return 'image';
    else if (['mp4', 'ogg', 'avi', 'mpeg'].includes(dataType))
      return 'video';
    else
      return 'download';
  }
}
</script>
