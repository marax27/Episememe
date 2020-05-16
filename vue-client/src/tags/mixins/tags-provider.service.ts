import { Component, Mixins } from 'vue-property-decorator';
import ApiClientService from '@/shared/mixins/api-client/api-client.service';
import { ITag } from '@/shared/models/ITag';
import { TagViewModel } from '../view-models/TagViewModel';

@Component
export default class TagsProviderService extends Mixins(ApiClientService) {

  created() {
    if (this.$store.state.tags == null) {
      this._loadTags();
    }
  }

  public get allTags(): TagViewModel[] {
    const tags: ITag[] = this.$store.getters.allTags;
    return tags.sort((a, b) => a.name.localeCompare(b.name))
      .map(tag => new TagViewModel(
        tag.name,
        `${tag.name} (${tag.description})`,
        tag.description
      ));
  }

  public refreshTags() {
    this._loadTags();
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
