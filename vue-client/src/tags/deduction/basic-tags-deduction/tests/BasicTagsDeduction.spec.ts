import * as ctx from './contexts';
import { ITagsDeduction } from '../../ITagsDeduction';
import { BasicTagsDeduction } from '../BasicTagsDeduction';

[
  new ctx.GivenNoTagsAndEmptyText(),
  new ctx.GivenNoTagsAndTextWithOneTag(),
  new ctx.GivenNoTagsAndTextWithManyTags(),
  new ctx.GivenSampleTagsAndEmptyText(),
  new ctx.GivenSampleTagsAndTextWithNewTags(),
  new ctx.GivenSampleTagsAndTextWithBothKnownAndNewTags(),
  new ctx.GivenSampleTagsAndTextThatMatchesMultipleTags(),
  new ctx.GivenSampleTagsAndTextWithPartialMatches()
].forEach((context: ctx.BasicTagsDeductionTestContext) => {

  describe(`BasicTagsDeduction test: ${context.constructor.name}`, () => {
    let sut: ITagsDeduction;

    beforeEach(() => {
      sut = new BasicTagsDeduction();
    });

    it('deduces expected names', () => {
      const actualResult = sut.deduceFrom(context.givenText, context.givenTags);
      expect(actualResult).toEqual(context.expectedTagNames);
    });
  });
});
