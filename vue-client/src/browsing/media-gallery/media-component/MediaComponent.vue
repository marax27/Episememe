<template>
  <img v-if='isImage()'
       v-show='active'
       :src='mediaUrl'
       :alt='altMessage'/>

  <video v-else-if='isVideo()'
         v-show='active'>
    <source :src='mediaUrl'
            :type='"video/" + instance.dataType'/>
    {{ altMessage }}
  </video>

  <div v-else
       v-show='active'>
    <p>{{ altMessage }}</p>
  </div>
</template>

<script lang='ts'>
import { Component, Prop, Vue } from 'vue-property-decorator';
import { IMediaInstance } from '../../../shared/models/IMediaInstance';

@Component
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

  public isImage(): boolean {
    const dataType = this.instance?.dataType.toLowerCase() ?? '';
    return ['jpg', 'png', 'bmp', 'svg', 'jpeg'].includes(dataType);
  }

  public isVideo(): boolean {
    const dataType = this.instance?.dataType.toLowerCase() ?? '';
    return ['mp4', 'ogg', 'avi', 'mpeg'].includes(dataType);
  }
}
</script>

<style scoped>

</style>
