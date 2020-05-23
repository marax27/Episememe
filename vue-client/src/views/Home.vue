<template>
  <ContentWrapper>
    <SearchPanel :loading='searchInProgress' @submit='onSubmit'></SearchPanel>
  </ContentWrapper>
</template>

<script lang='ts'>
import { Component, Mixins } from 'vue-property-decorator';
import router from '../router';
import ContentWrapper from '../shared/components/content-wrapper/ContentWrapper.vue';
import ApiClientService from '../shared/mixins/api-client/api-client.service';
import SearchPanel from '../searching/SearchPanel.vue';
import { ISearchSpecification } from '../searching/interfaces/ISearchSpecification';
import { SearchSpecificationDto } from '../searching/interfaces/SearchSpecificationDto';

@Component({
  name: 'Home',
  components: {
    ContentWrapper,
    SearchPanel
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
      timeRangeEnd: specification.timeTo
    };
    return JSON.stringify(obj);
  }
}
</script>
