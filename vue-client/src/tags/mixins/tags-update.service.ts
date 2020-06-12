import { Component, Mixins } from 'vue-property-decorator';
import ApiClientService from '@/shared/mixins/api-client/api-client.service';

export interface UpdateTagDto {
  name: string;
  description?: string;
  children: string[];
  parents: string[];
}

@Component
export default class TagsUpdateService extends Mixins(ApiClientService) {

  public updateTag(tagName: string, data: UpdateTagDto): Promise<void> {
    const resource = `tags/${tagName}`;
    return this.$api.put<void>(resource, data)
      .then(response => response.data);
  }
}
