import { ITag } from '@/shared/models/ITag';

const EmptyTags: ITag[] = [];
const SampleTags: ITag[] = [
  { name: 'Sample Tag', description: '' },
  { name: 'Another Tag', description: '' }
];

export interface BasicTagsDeductionTestContext {
  givenTags: ITag[];
  givenText: string;

  expectedTagNames: string[];
}

export class GivenNoTagsAndEmptyText implements BasicTagsDeductionTestContext {
  givenTags = EmptyTags;
  givenText = '';
  expectedTagNames = [];
}

export class GivenNoTagsAndTextWithOneTag implements BasicTagsDeductionTestContext {
  givenTags = EmptyTags;
  givenText = 'only-tag';
  expectedTagNames = ['only tag'];
}

export class GivenNoTagsAndTextWithManyTags implements BasicTagsDeductionTestContext {
  givenTags = EmptyTags;
  givenText = 'tag1 Tag2 TAG3 yet-another-tag';
  expectedTagNames = ['tag1', 'Tag2', 'TAG3', 'yet another tag'];
}

export class GivenSampleTagsAndEmptyText implements BasicTagsDeductionTestContext {
  givenTags = SampleTags;
  givenText = '';
  expectedTagNames = [];
}

export class GivenSampleTagsAndTextWithNewTags implements BasicTagsDeductionTestContext {
  givenTags = SampleTags;
  givenText = 'something-new 12345';
  expectedTagNames = ['something new', '12345'];
}

export class GivenSampleTagsAndTextWithBothKnownAndNewTags implements BasicTagsDeductionTestContext {
  givenTags = SampleTags;
  givenText = 'unknown another-tag another-unknown';
  expectedTagNames = ['unknown', 'Another Tag', 'another unknown'];
}

export class GivenSampleTagsAndTextThatMatchesMultipleTags implements BasicTagsDeductionTestContext {
  givenTags = SampleTags;
  givenText = 'tag';
  expectedTagNames = ['tag'];
}

export class GivenSampleTagsAndTextWithPartialMatches implements BasicTagsDeductionTestContext {
  givenTags = SampleTags;
  givenText = 'another sample';
  expectedTagNames = ['Another Tag', 'Sample Tag'];
}
