import { ITagsDeduction } from './ITagsDeduction';
import { ITag } from '@/shared/models/ITag';

export class BasicTagsDeduction implements ITagsDeduction {

  public deduceFrom(text: string, knownTags: ITag[]): string[] {
    const parts = text
      .split(' ')
      .map(s => s.split('-').join(' '));

    return parts
      .map(s => this.getMatchingTagName(s, knownTags) ?? s);
  }

  private getMatchingTagName(name: string, allTags: ITag[]): string | null {
    const exactMatch = allTags
      .find(tag => tag.name === name);
    if (exactMatch != undefined)
      return exactMatch.name;

    const lowercaseName = name.toLocaleLowerCase();

    const caseInsensitiveMatch = allTags
      .find(tag => tag.name.toLocaleLowerCase() === lowercaseName);
    if (caseInsensitiveMatch != undefined)
      return caseInsensitiveMatch.name;

    const partialMatches = allTags
      .filter(
        tag => tag.name.toLocaleLowerCase().includes(lowercaseName)
            || tag.description?.toLocaleLowerCase().includes(lowercaseName)
      );
    if (partialMatches.length === 1)
      return partialMatches[0].name;

    return null;
  }
}
