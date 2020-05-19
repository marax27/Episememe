import { Component, Mixins } from 'vue-property-decorator';
import TagsProviderService from './tags-provider.service';
import { ITagsDeduction } from '../deduction/ITagsDeduction';
import { BasicTagsDeduction } from '../deduction/basic-tags-deduction/BasicTagsDeduction';

@Component
export default class TagsDeductionService extends Mixins(TagsProviderService) {

  private _tagsDeduction!: ITagsDeduction;

  beforeCreate() {
    this._tagsDeduction = new BasicTagsDeduction();
  }

  public deduceTagsForFile(file: File): string[] {
    const clearName = this.truncateFileExtension(file.name);
    return this._tagsDeduction.deduceFrom(clearName, this.allTags);
  }

  private truncateFileExtension(filename: string): string {
    if (filename.indexOf('.') === -1) {
      return filename;
    }
    return filename.split('.').slice(0, -1).join('.');
  }
}
