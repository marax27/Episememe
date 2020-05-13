import { Component, Mixins } from 'vue-property-decorator';
import ApiClientService from '@/shared/mixins/api-client/api-client.service';
import { ITag } from '@/shared/models/ITag';

@Component
export default class TagsProviderService extends Mixins(ApiClientService) {

  created() {
    if (this.$store.state.tags == null) {
      this._loadTags();
    }
  }

  public get allTags(): ITag[] {
    const tags: ITag[] = this.$store.getters.allTags;
    return tags.sort((a, b) => a.name.localeCompare(b.name));
  }

  private _loadTags() {
    this.$api.get<ITag[]>('tags')
      .then(response => {
        this.$store.dispatch('updateTags', response.data);
      }).catch(err => {
        this.$store.dispatch('reportError', 'Failed to load data from the server.');
      });
  }
}
