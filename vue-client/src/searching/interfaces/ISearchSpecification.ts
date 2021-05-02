export interface ISearchSpecification {
    includeTags: string[];
    excludeTags: string[];
    timeFrom: Date | null;
    timeTo: Date | null;
    favoritesOnly: boolean;
}
