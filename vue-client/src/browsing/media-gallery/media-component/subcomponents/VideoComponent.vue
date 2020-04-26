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
    const el = this.$refs.videoRef as HTMLMediaElement;
    el.load();
    if (value) {
      el.play();
    }
  }
}
</script>
