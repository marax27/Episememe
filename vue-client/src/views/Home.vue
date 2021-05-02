<template>
  <ContentWrapper>
    <SearchPanel
      :loading='searchInProgress'
      @submit='onSubmit'
      class='home-panel' />

    <v-row>
      <v-spacer></v-spacer>
      <v-col cols='12' sm='6'>
        <StatisticsPanel
          class='home-panel' />
      </v-col>
    </v-row>
  </ContentWrapper>
</template>

<script lang='ts'>
import { Component, Mixins } from 'vue-property-decorator';
import router from '../router';
import ContentWrapper from '../shared/components/content-wrapper/ContentWrapper.vue';
import ApiClientService from '../shared/mixins/api-client/api-client.service';
import SearchPanel from '../searching/SearchPanel.vue';
import StatisticsPanel from '../statistics/StatisticsPanel.vue';
import { ISearchSpecification } from '../searching/interfaces/ISearchSpecification';
import { SearchSpecificationDto } from '../searching/interfaces/SearchSpecificationDto';

@Component({
  name: 'Home',
  components: {
    ContentWrapper,
    SearchPanel,
    StatisticsPanel,
  }
})
export default class Home extends Mixins(ApiClientService) {

  searchInProgress = false;

  onSubmit(specification: ISearchSpecification) {
    const galleryData = this.createGalleryData(specification);

    this.searchInProgress = true;
    router.push({ name: 'Gallery', params: { data: galleryData } });
  }

  private createGalleryData(specification: ISearchSpecification): string {
    const obj: SearchSpecificationDto = {
      includedTags: specification.includeTags,
      excludedTags: specification.excludeTags,
      timeRangeStart: specification.timeFrom,
      timeRangeEnd: specification.timeTo,
      favoritesOnly: specification.favoritesOnly
    };
    return JSON.stringify(obj);
  }
}
</script>

<style scoped>
.home-panel + .home-panel {
  margin-top: 1em;
}
</style>
