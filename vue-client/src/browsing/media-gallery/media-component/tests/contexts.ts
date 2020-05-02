import { IMediaInstance } from '@/shared/models/IMediaInstance';

const SampleImageInstance: IMediaInstance = {
  id: '12345678',
  dataType: 'jpg',
  tags: []
};

const SampleVideoInstance: IMediaInstance = {
  id: '12345678',
  dataType: 'mp4',
  tags: []
};

const SamplePdfInstance: IMediaInstance = {
  id: '12345678',
  dataType: 'pdf',
  tags: []
};

export interface MediaComponentTestContext {
  givenInstance: IMediaInstance;
  givenActive: boolean;

  expectedElementSelector: string;
  expectedVisible: boolean;
}

export class GivenSampleImage implements MediaComponentTestContext {
  givenInstance = SampleImageInstance;
  givenActive = true;

  expectedElementSelector = 'img';
  expectedVisible = true;
}

export class GivenSampleVideo implements MediaComponentTestContext {
  givenInstance = SampleVideoInstance;
  givenActive = true;

  expectedElementSelector = 'video';
  expectedVisible = true;
}

export class GivenSamplePdf implements MediaComponentTestContext {
  givenInstance = SamplePdfInstance;
  givenActive = true;

  expectedElementSelector = 'button';
  expectedVisible = true;
}

export class GivenInactiveImage implements MediaComponentTestContext {
  givenInstance = SampleImageInstance;
  givenActive = false;

  expectedElementSelector = 'img';
  expectedVisible = false;
}

export class GivenInactiveVideo implements MediaComponentTestContext {
  givenInstance = SampleVideoInstance;
  givenActive = false;

  expectedElementSelector = 'video';
  expectedVisible = false;
}

export class GivenInactivePdf implements MediaComponentTestContext {
  givenInstance = SamplePdfInstance;
  givenActive = false;

  expectedElementSelector = 'button';
  expectedVisible = false;
}
