import { IMediaInstance } from '@/shared/models/IMediaInstance';

export interface MediaGalleryTestContext {
    givenInstances: IMediaInstance[];
    shouldDisplayEmptyQueryWarning: boolean;
}

export class GivenNoMediaInstances implements MediaGalleryTestContext {
    givenInstances = [];
    shouldDisplayEmptyQueryWarning = true;
}

export class GivenSingleMediaInstance implements MediaGalleryTestContext {
    givenInstances = [{ id: '12345678', dataType: 'jpg', tags: [] }];
    shouldDisplayEmptyQueryWarning = false;
}

export class GivenSeveralMediaInstances implements MediaGalleryTestContext {
    givenInstances = [
        { id: '12345678', dataType: 'jpg', tags: [] },
        { id: '87654321', dataType: 'png', tags: [] },
        { id: 'abcdefgh', dataType: 'gif', tags: [] }
    ];
    shouldDisplayEmptyQueryWarning = false;
}
