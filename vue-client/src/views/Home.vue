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
import { ISearchSpecification } from '../shared/models/ISearchSpecification';

@Component({
  name: 'Home',
  components: {
    ContentWrapper,
    SearchPanel
  }
})
export default class Home extends Mixins(ApiClientService) {

  onSubmit(specification: ISearchSpecification) {
    console.dir({ specs: specification });
    // this.refreshBrowseToken(() => {
      // router.push({ name: 'Gallery', params: { data: '123' } });
    // });
  }

  private refreshBrowseToken(afterCompletion: () => void) {
    return this.$api
      .post<string>('authorization', {})
      .then(response => {
        this.$store.dispatch('refreshBrowseToken', response.data)
          .then(_ => afterCompletion());
      })
      .catch(err => {
        console.error(`Failed to retrieve the browser token.`);
      });
  }
}
</script>
