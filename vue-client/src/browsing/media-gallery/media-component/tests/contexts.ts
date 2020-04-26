import { IMediaInstance } from '@/shared/models/IMediaInstance';

export interface MediaComponentTestContext {
  givenInstance: IMediaInstance;
  givenActive: boolean;
  expectedElementSelector: string;
}

export class GivenSampleImage implements MediaComponentTestContext {
  givenInstance = {
    id: '12345678',
    dataType: 'jpg',
    tags: []
  };
  givenActive = true;

  expectedElementSelector = 'img';
}

export class GivenSampleVideo implements MediaComponentTestContext {
  givenInstance = {
    'id': '12345678',
    dataType: 'mp4',
    tags: []
  };
  givenActive = true;

  expectedElementSelector = 'video';
}
