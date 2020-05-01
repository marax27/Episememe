import ApiClient from "../api-client";

describe('ApiClient Test: URL creation', () => {
  const givenApiUrl = 'http://example/api';
  const givenResource = 'sample';
  let sut: ApiClient;

  beforeEach(() => {
    sut = new ApiClient({}, givenApiUrl);
  });

  [
    {
      parameters: { a: '123', b: 'xyz' },
      expectedResult: 'http://example/api/sample?a=123&b=xyz'
    },
    {
      parameters: { a: '123' },
      expectedResult: 'http://example/api/sample?a=123'
    },
    {
      parameters: {},
      expectedResult: 'http://example/api/sample'
    },
    {
      parameters: { x: '   ' },
      expectedResult: 'http://example/api/sample?x=%20%20%20'
    }
  ].forEach(context => {
    it(`creates an expected URL for ${JSON.stringify(context.parameters)}`, () => {
      const givenParameters = context.parameters as any;
      const actualResult = sut.createUrl(givenResource, givenParameters);
      expect(actualResult).toBe(context.expectedResult);
    });
  });
});
