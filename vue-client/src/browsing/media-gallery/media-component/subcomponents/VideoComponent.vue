<template>
  <video>
    <source :src='url' :type='"video/" + type'/>
    {{ alt }}
  </video>
</template>

<script lang='ts'>
import { Component, Prop, Vue, Watch } from 'vue-property-decorator';
import { IMediaInstance } from '../../../../shared/models/IMediaInstance';

@Component
export default class VideoComponent extends Vue {
  @Prop()
  url?: string;

  @Prop()
  type?: string;

  @Prop()
  alt?: string;
}
</script>
