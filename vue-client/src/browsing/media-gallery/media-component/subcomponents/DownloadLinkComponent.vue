<template>
  <div>
    <p>{{ "#" + identifier }}</p>
    <p>Download</p>
  </div>
</template>

<script lang='ts'>
import { Component, Prop, Vue } from 'vue-property-decorator';
import { IMediaInstance } from '../../../../shared/models/IMediaInstance';

@Component
export default class DownloadLinkComponent extends Vue {
  @Prop()
  identifier?: IMediaInstance;
}
</script>
