import { shallowMount } from '@vue/test-utils';
import vuetify from 'vuetify';
import Vue from 'vue';
import MediaComponent from '../MediaComponent.vue';
import * as ctx from './contexts';

[
  new ctx.GivenSampleImage()
].forEach(context => {

  describe(`MediaComponent Test: ${context.constructor.name}`, () => {
    let wrapper: ReturnType<typeof shallowMount>;
  
    beforeAll(() => {
      Vue.use(vuetify);
    });
  
    beforeEach(() => {
      wrapper = shallowMount(MediaComponent, {
        propsData: {
          instance: context.givenInstance
        }
      })
    });

    it('is a Vue instance', () => {
      expect(wrapper.isVueInstance()).toBeTruthy();
    });

    it('displays an expected element', () => {
      const element = wrapper.find(context.expectedElementSelector);
      expect(element.isVisible()).toBeTruthy();
    });
  });
});
