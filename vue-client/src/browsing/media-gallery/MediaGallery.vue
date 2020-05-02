<template>
  <div class='wrapper'>

    <div v-if='isQueryEmpty' class='media-gallery'>
      <p>Search query is empty</p>
    </div>
    <div v-else class='media-gallery'>
      <MediaComponent
        v-for='(item, index) in instances' :key='item.id'
        :active='index === currentlyBrowsedIndex'
        :instance='item'
        class='media-instance'>
      </MediaComponent>
    </div>

    <v-btn class='previous-instance' x-large icon color='secondary'
           @click='movePrevious'>
      <v-icon>mdi-arrow-left</v-icon>
    </v-btn>

    <v-btn class='next-instance' x-large icon color='secondary'
           @click='moveNext'>
      <v-icon>mdi-arrow-right</v-icon>
    </v-btn>

  </div>
</template>

<script lang='ts'>
import { Component, Prop, Vue } from 'vue-property-decorator';
import MediaComponent from './media-component/MediaComponent.vue';
import { IMediaInstance } from '../../shared/models/IMediaInstance';

@Component({
  components: {
    'MediaComponent': MediaComponent
  }
})
export default class MediaGallery extends Vue {
  @Prop({ default: [] })
  instances!: IMediaInstance[];

  public get isQueryEmpty(): boolean {
    return this.instances == null || this.instances.length === 0;
  }

  currentlyBrowsedIndex = 0;

  movePrevious() {
    --this.currentlyBrowsedIndex;
    if (this.currentlyBrowsedIndex < 0)
      this.currentlyBrowsedIndex += this.instances.length;
  }

  moveNext() {
    ++this.currentlyBrowsedIndex;
    if (this.currentlyBrowsedIndex >= this.instances.length)
      this.currentlyBrowsedIndex -= this.instances.length;
  }
}
</script>

<style scoped>
.wrapper, .media-gallery {
  width: 100%;
  height: 100%;
  margin: 0;
  padding: 0;
}

.media-gallery {
  background-color: black;

  display: flex;
  align-items: center;
  justify-content: center;
}

.media-gallery .media-instance {
  object-fit: contain;
  max-height: 90vh;
}

.previous-instance, .next-instance {
  position: fixed;
  top: 50%;
}

.previous-instance { left: 0; }
.next-instance { right: 0; }

.previous-instance:hover, .next-instance:hover {
  color: white !important;
}
</style>
