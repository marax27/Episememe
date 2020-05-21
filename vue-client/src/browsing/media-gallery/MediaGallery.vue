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

    <v-btn class='show-tags' x-large icon color='secondary'
      @click='sidebarOpen = true'>
      <v-icon>mdi-information-outline</v-icon>
    </v-btn>

    <MediaSidebar
      v-model='sidebarOpen'
      :instance='instances[currentlyBrowsedIndex]' />

  </div>
</template>

<script lang='ts'>
import { Component, Prop, Vue, Watch } from 'vue-property-decorator';
import MediaComponent from './media-component/MediaComponent.vue';
import MediaSidebar from './media-sidebar/MediaSidebar.vue';
import { IMediaInstance } from '../../shared/models/IMediaInstance';

@Component({
  components: {
    MediaComponent,
    MediaSidebar
  }
})
export default class MediaGallery extends Vue {
  @Prop({ default: [] })
  instances!: IMediaInstance[];

  sidebarOpen = false;

  public get isQueryEmpty(): boolean {
    return this.instances == null || this.instances.length === 0;
  }

  private updateInstance() {
    const currentInstance = this.instances[this.currentlyBrowsedIndex];
    this.$store.dispatch('gallery/updateCurrentMediaInstance', currentInstance);
  }

  currentlyBrowsedIndex = 0;

  movePrevious() {
    --this.currentlyBrowsedIndex;
    if (this.currentlyBrowsedIndex < 0)
      this.currentlyBrowsedIndex += this.instances.length;
    this.updateInstance();
  }

  moveNext() {
    ++this.currentlyBrowsedIndex;
    if (this.currentlyBrowsedIndex >= this.instances.length)
      this.currentlyBrowsedIndex -= this.instances.length;
    this.updateInstance();
  }

  @Watch('instances')
  private onInstancesChange() {
    // Necessary to detect when instances are first loaded.
    this.updateInstance();
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

.previous-instance:hover, .next-instance:hover, .show-tags:hover {
  color: white !important;
}

.show-tags {
  position: absolute;
  top: 0;
  right: 0;
}
</style>
