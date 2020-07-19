<template>
  <img :src='url' :alt='alt' ref='imageRef'/>
</template>

<script lang='ts'>
import { Component, Prop, Vue } from 'vue-property-decorator';
import { IHasResolution } from '../../../interfaces/IHasResolution';
import { IResolution } from '../../../interfaces/IResolution';

@Component
export default class ImageComponent extends Vue implements IHasResolution {
  @Prop()
  url?: string;

  @Prop()
  alt?: string;

  private resolution: IResolution | null = null;

  public getResolution(): Promise<IResolution> {
    const image = this.image;
    if (image.complete) {
      const result = this.updateResolution(image.naturalWidth, image.naturalHeight);
      return Promise.resolve(result);
    } else {
      // Note: might potentially cause issues if the function
      // is called 2 times while an image is not yet loaded.
      return new Promise((resolve, reject) => {
        image.onload = () => {
          const result = this.updateResolution(image.naturalWidth, image.naturalHeight);
          resolve(result);
        }
        image.onerror = reject;
      });
    }
  }

  private get image() {
    return this.$refs.imageRef as HTMLImageElement;
  }

  private updateResolution(width: number, height: number): IResolution {
    const result = { width: width, height: height };
    this.resolution = result;
    return result;
  }
}
</script>
