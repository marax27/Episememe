<template>
  <ContentWrapper>
    <SearchPanel @submit='onSubmit'></SearchPanel>
  </ContentWrapper>
</template>

<script lang='ts'>
import { Component, Vue, Mixins } from 'vue-property-decorator';
import router from '../router';
import ContentWrapper from '../shared/components/content-wrapper/ContentWrapper.vue';
import ApiClientService from '../shared/mixins/api-client/api-client.service';
import SearchPanel from '../searching/SearchPanel.vue';
import { ISearchSpecification } from '../searching/interfaces/ISearchSpecification';
import { SearchSpecificationDto } from '../searching/interfaces/SearchSpecificationDto';
import { ITag } from '../shared/models/ITag';

@Component({
  name: 'Home',
  components: {
    ContentWrapper,
    SearchPanel
  }
})
export default class Home extends Mixins(ApiClientService) {

  created() {
    if (this.$store.state.tags == null) {
      this.loadTags();
    }
  }

  onSubmit(specification: ISearchSpecification) {
    const galleryData = this.createGalleryData(specification);
    this.refreshBrowseToken(() => {
      router.push({ name: 'Gallery', params: { data: galleryData } });
    });
  }

  private refreshBrowseToken(afterCompletion: () => void) {
    return this.$api
      .post<string>('authorization', {})
      .then(response => {
        this.$store.dispatch('refreshBrowseToken', response.data)
          .then(_ => afterCompletion());
      })
      .catch(err => {
        console.error('Failed to retrieve the browse token.');
        if (this.$store.state.browseToken != null) {
          afterCompletion();
        }
      });
  }

  private loadTags() {
    this.$api.get<ITag[]>('tags')
      .then(response => { this.$store.dispatch('updateTags', response.data); })
      .catch(err => console.error(`Failed to load tags from the API: ${err}.`));
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
