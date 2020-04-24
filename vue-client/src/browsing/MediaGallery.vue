<template>
  <div class='wrapper'>

    <div class='media-gallery'>
      <img v-for='(item, index) in mediaInstances' :key='item.id'
          class='media-instance'
          v-bind:class='{ "currently-browsed": index === currentlyBrowsedIndex }'
          :src='item.address' alt='Cannot display media'/>
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

@Component
export default class MediaGallery extends Vue {
  mediaInstances: any[] = [
    { id: '1', address: 'http://localhost:18888/0.jpg' },
    { id: '2', address: 'http://localhost:18888/1.jpg' },
    { id: '3', address: 'http://localhost:18888/2.pdf' },
    { id: '4', address: 'http://localhost:18888/3.jpg' },
    { id: '5', address: 'http://localhost:18888/4.jpg' },
    { id: '6', address: 'http://localhost:18888/5.mp4' },
  ];

  currentlyBrowsedIndex = 0;

  movePrevious() {
    --this.currentlyBrowsedIndex;
    if (this.currentlyBrowsedIndex < 0)
      this.currentlyBrowsedIndex += this.mediaInstances.length;
  }

  moveNext() {
    ++this.currentlyBrowsedIndex;
    if (this.currentlyBrowsedIndex >= this.mediaInstances.length)
      this.currentlyBrowsedIndex -= this.mediaInstances.length;
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
  display: none;
  object-fit: contain;
  max-height: 90vh;
}

.media-gallery .currently-browsed.media-instance {
  display: unset;
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
