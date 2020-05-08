
export interface SearchSpecificationDto {
  includedTags: string[];
  excludedTags: string[];
  timeRangeStart: Date | null;
  timeRangeEnd: Date | null;
}
