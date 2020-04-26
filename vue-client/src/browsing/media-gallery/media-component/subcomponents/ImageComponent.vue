<template>
  <img :src='url' :alt='alt'/>
</template>

<script lang='ts'>
import { Component, Prop, Vue } from 'vue-property-decorator';

@Component
export default class ImageComponent extends Vue {
  @Prop()
  url?: string;

  @Prop()
  alt?: string;
}
</script>
