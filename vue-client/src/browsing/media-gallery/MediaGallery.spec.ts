import { shallowMount } from '@vue/test-utils';
import VueRouter from 'vue-router';
import vuetify from 'vuetify';
import Vue from 'vue';
import MediaGallery from './MediaGallery.vue';
import * as ctx from './test-contexts';

[
  new ctx.GivenNoMediaInstances(),
  new ctx.GivenSingleMediaInstance()
].forEach(context => 
  describe(`MediaGallery Test: ${context.constructor.name}`, () => {

    let wrapper: ReturnType<typeof shallowMount>;

    beforeAll(() => {
      Vue.use(vuetify);
      Vue.use(VueRouter);
    });

    beforeEach(() => {
      wrapper = shallowMount(MediaGallery, {
        propsData: {
          instances: context.givenInstances
        }
      });
    });

    it('is a Vue instance', () => {
      expect(wrapper.isVueInstance()).toBeTruthy();
    });

    it('contains an expected number of media components', () => {
      expect(wrapper.findAll('.media-instance').length)
        .toEqual(context.givenInstances.length);
    });

    it('contains no more than one visible media component', () => {
      const allInstances = wrapper.findAll('.media-instance');
      const visibleInstances = allInstances.filter(instance => instance.isVisible());
      expect(visibleInstances.length).toBeLessThanOrEqual(1);
    });

    const shouldContainWarning = context.shouldDisplayEmptyQueryWarning;
    it(`should ${shouldContainWarning ? "" : "not "}display an "empty query" warning`, () => {
      const warningPhrase = 'Search query is empty';
      const containsWarning = wrapper.text().includes(warningPhrase);

      expect(containsWarning).toBe(shouldContainWarning);
    });
  })
);
