import { mount } from '@vue/test-utils';
import vuetify from 'vuetify';
import Vue from 'vue';
import MediaComponent from '../MediaComponent.vue';
import * as ctx from './contexts';

[
  new ctx.GivenSampleImage(),
  new ctx.GivenSampleVideo()
].forEach(context => {

  describe(`MediaComponent Test: ${context.constructor.name}`, () => {
    let wrapper: ReturnType<typeof mount>;
  
    beforeAll(() => {
      Vue.use(vuetify);
    });
  
    beforeEach(() => {
      wrapper = mount(MediaComponent, {
        propsData: {
          instance: context.givenInstance,
          active: context.givenActive
        }
      })
    });

    it('is a Vue instance', () => {
      expect(wrapper.isVueInstance()).toBeTruthy();
    });

    it('contains an expected element', () => {
      const element = wrapper.find(context.expectedElementSelector);
      expect(element.exists()).toBeTruthy();
    })

    it('displays an expected element', () => {
      const element = wrapper.find(context.expectedElementSelector);
      expect(element.isVisible()).toBeTruthy();
    });
  });
});
