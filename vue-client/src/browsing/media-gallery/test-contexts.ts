
export interface MediaGalleryTestContext {
    givenInstances: any[];
    shouldDisplayEmptyQueryWarning: boolean;
}

export class GivenNoMediaInstances implements MediaGalleryTestContext {
    givenInstances = [];
    shouldDisplayEmptyQueryWarning = true;
}

export class GivenSingleMediaInstance implements MediaGalleryTestContext {
    givenInstances = [{id: 'abc', address: '/123'}];
    shouldDisplayEmptyQueryWarning = false;
}
