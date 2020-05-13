<template>
  <ContentWrapper>
    <SearchPanel :loading='searchInProgress' @submit='onSubmit'></SearchPanel>
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

  searchInProgress = false;

  created() {
    if (this.$store.state.tags == null) {
      this.loadTags();
    }
  }

  onSubmit(specification: ISearchSpecification) {
    const galleryData = this.createGalleryData(specification);

    this.searchInProgress = true;
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
        if (this.$store.state.browseToken != null) {
          afterCompletion();
        } else {
          this.reportError();
        }
      });
  }

  private loadTags() {
    this.$api.get<ITag[]>('tags')
      .then(response => { this.$store.dispatch('updateTags', response.data); })
      .catch(err => this.reportError());
  }

  private reportError() {
    this.$store.dispatch('reportError', 'Failed to retrieve data from the server.');
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
