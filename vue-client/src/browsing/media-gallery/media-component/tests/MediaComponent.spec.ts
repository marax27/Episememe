import { mount } from '@vue/test-utils';
import vuetify from 'vuetify';
import Vuex, { Store } from 'vuex';
import Vue from 'vue';
import MediaComponent from '../MediaComponent.vue';
import * as ctx from './contexts';

const apiClientMock = {
  createUrl: (_x: any, _y: any) => 'dumb-url.com'
};

[
  new ctx.GivenSampleImage(), 
  new ctx.GivenSampleVideo(),
  new ctx.GivenSamplePdf(),
  new ctx.GivenInactiveImage(),
  new ctx.GivenInactiveVideo(),
  new ctx.GivenInactivePdf(),
].forEach(context => {

  describe(`MediaComponent Test: ${context.constructor.name}`, () => {
    let wrapper: ReturnType<typeof mount>;
    let store: Store<any>;
  
    beforeAll(() => {
      Vue.use(vuetify);
      Vue.use(Vuex);

      store = new Vuex.Store({
        state: {
          browseToken: 'sampleToken',
          gallery: {
            state: { isMuted: true, autoloop: false }
          }
        },
      });
    });
  
    beforeEach(() => {
      wrapper = mount(MediaComponent, {
        store,
        mocks: {
          '$api': apiClientMock
        },
        propsData: {
          instance: context.givenInstance,
          active: context.givenActive
        }
      });
    });

    it('is a Vue instance', () => {
      expect(wrapper.isVueInstance()).toBeTruthy();
    });

    it('contains an expected element', () => {
      const element = wrapper.find(context.expectedElementSelector);
      expect(element.exists()).toBeTruthy();
    })

    const displayMessage = context.expectedVisible ? 'displays' : 'does not display';
    it(`${displayMessage} an expected element`, () => {
      const element = wrapper.find(context.expectedElementSelector);
      expect(element.isVisible()).toBe(context.expectedVisible);
    });
  });
});
