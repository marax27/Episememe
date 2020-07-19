<template>
  <img :src='url' :alt='alt' ref='imageRef'/>
</template>

<script lang='ts'>
import { Component, Prop, Vue } from 'vue-property-decorator';
import { IHasResolution } from '../../../interfaces/IHasResolution';
import { ResolutionModes } from '../../../types/ResolutionModes';

@Component
export default class ImageComponent extends Vue implements IHasResolution {
  @Prop()
  url?: string;

  @Prop()
  alt?: string;

  private resolutionMode: ResolutionModes = ResolutionModes.Unknown;

  public getResolution(): Promise<ResolutionModes> {
    const image = this.image;
    if (image.complete) {
      this.updateResolutionMode(image.naturalWidth, image.naturalHeight);
      return Promise.resolve(this.resolutionMode);
    } else {
      // Note: might potentially cause issues if the function
      // is called 2 times while an image is not yet loaded.
      return new Promise((resolve, reject) => {
        image.onload = () => {
          this.updateResolutionMode(image.naturalWidth, image.naturalHeight);
          resolve(this.resolutionMode);
        }
        image.onerror = reject;
      });
    }
  }

  private get image() {
    return this.$refs.imageRef as HTMLImageElement;
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
