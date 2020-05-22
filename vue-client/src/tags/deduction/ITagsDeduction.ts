import { ITag } from '@/shared/models/ITag';

export interface ITagsDeduction {
  deduceFrom(text: string, knownTags: ITag[]): string[];
}
