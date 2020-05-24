import { shallowMount } from '@vue/test-utils';
import vuetify from 'vuetify';
import Vue from 'vue';
import MediaGallery from '../MediaGallery.vue';
import MediaComponent from '../media-component/MediaComponent.vue';
import * as ctx from './contexts';
import Vuex, { Store } from 'vuex';

[
  new ctx.GivenNoMediaInstances(),
  new ctx.GivenSingleMediaInstance(),
  new ctx.GivenSeveralMediaInstances()
].forEach(context => 
  describe(`MediaGallery Test: ${context.constructor.name}`, () => {

    let wrapper: ReturnType<typeof shallowMount>;
    let store: Store<any>;
    let updateActionMock: jest.Mock;

    beforeAll(() => {
      Vue.use(vuetify);
      Vue.use(Vuex);
    });

    beforeEach(() => {
      updateActionMock = jest.fn();

      store = new Vuex.Store({
        modules: {
          gallery: {
            namespaced: true,
            state: { currentMediaInstance: null },
            actions: { updateCurrentMediaInstance: updateActionMock }
          }
        }
      });
    });

    beforeEach(() => {
      wrapper = shallowMount(MediaGallery, {
        store,
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
      const allInstances = wrapper.findAll(MediaComponent);
      const visibleInstances = allInstances.filter(instance => instance.props('active'));
      expect(visibleInstances.length).toBeLessThanOrEqual(1);
    });

    const prefix = context.shouldUpdateStore ? 'updates' : 'does not update';
    it(`${prefix} an in-store media instance once upon creation`, () => {
      const expectedUpdateCount = context.shouldUpdateStore ? 1 : 0;
      expect(updateActionMock).toHaveBeenCalledTimes(expectedUpdateCount);
    });

    const shouldContainWarning = context.shouldDisplayEmptyQueryWarning;
    it(`should ${shouldContainWarning ? "" : "not "}display an "empty query" warning`, () => {
      const warningPhrase = 'Search query is empty';
      const containsWarning = wrapper.text().includes(warningPhrase);

      expect(containsWarning).toBe(shouldContainWarning);
    });
  })
);
