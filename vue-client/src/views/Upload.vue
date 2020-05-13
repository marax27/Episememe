<template>
  <ContentWrapper>
    <UploadPanel/>
  </ContentWrapper>
</template>


<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import ContentWrapper from '../shared/components/content-wrapper/ContentWrapper.vue';
import UploadPanel from '../upload/UploadPanel.vue';

@Component({
  name: 'Upload',
  components: {
    ContentWrapper,
    UploadPanel
  }
})
export default class Upload extends Vue {}
</script>