import { IMediaInstance } from '@/shared/models/IMediaInstance';

export interface MediaComponentTestContext {
  givenInstance: IMediaInstance;
  expectedElementSelector: string;
}

export class GivenSampleImage implements MediaComponentTestContext {
  givenInstance = {
    id: '12345678',
    dataType: 'jpg',
    tags: []
  };
  expectedElementSelector = 'img'
}
