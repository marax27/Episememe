<template>
  <ContentWrapper>
    <v-card>
      Revision page
    </v-card>
  </ContentWrapper>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import ContentWrapper from '../shared/components/content-wrapper/ContentWrapper.vue';

@Component({
  name: 'Revision',
  components: {
    ContentWrapper
  }
})
export default class Revision extends Vue {}
</script>
