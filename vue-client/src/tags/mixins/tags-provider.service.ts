import { Component, Mixins } from 'vue-property-decorator';
import ApiClientService from '@/shared/mixins/api-client/api-client.service';
import { ITag } from '@/shared/models/ITag';
import { TagViewModel } from '../view-models/TagViewModel';

@Component
export default class TagsProviderService extends Mixins(ApiClientService) {

  created() {
    if (this.$store.state.tags == null) {
      this.waitForAuthService()
        .then(
          _onSuccess => this._loadTags(),
          _onFailure => this._reportError('Failed to load data from the server: initialization error.')
        );
    }
  }

  public get allTags(): TagViewModel[] {
    return this.$store.getters.allTags;
  }

  public refreshTags() {
    this._loadTags();
  }

  private _loadTags() {
    this.$api.get<ITag[]>('tags')
      .then(response => this._mapTags(response.data))
      .then((tags: TagViewModel[]) => {
        this.$store.dispatch('updateTags', tags);
      }).catch(_err => {
        this._reportError('Failed to load data from the server.');
      });
  }

  private _mapTags(tags: ITag[]) {
    return tags.sort((a, b) => a.name.localeCompare(b.name))
      .map(tag => new TagViewModel(
        tag.name,
        `${tag.name}` + (tag.description ? ` (${tag.description})` : ''),
        tag.children,
        tag.parents,
        tag.description
      ));
  }

  private _reportError(message: string) {
    this.$store.dispatch('reportError', message);
  }
}
