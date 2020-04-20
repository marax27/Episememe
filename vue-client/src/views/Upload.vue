<template>
  <ContentWrapper>
    <v-card>
      Upload page
    </v-card>
  </ContentWrapper>
</template>


<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import ContentWrapper from '../shared/components/content-wrapper/ContentWrapper.vue';

@Component({
  name: 'Upload',
  components: {
    ContentWrapper
  }
})
export default class Upload extends Vue {}
</script>