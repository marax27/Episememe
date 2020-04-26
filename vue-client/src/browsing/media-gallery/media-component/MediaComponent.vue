<template>
  <img :src='mediaUrl'
       :alt='`Cannot display #${instance.id}`'/>
</template>

<script lang='ts'>
import { Component, Prop, Vue } from 'vue-property-decorator';
import { IMediaInstance } from '../../../shared/models/IMediaInstance';

@Component
export default class MediaComponent extends Vue {
  @Prop()
  instance?: IMediaInstance;

  public get mediaUrl(): string {
    return 'http://localhost:18888/' + this.instance?.id;
  }
}
</script>

<style scoped>

</style>
