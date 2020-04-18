import { shallowMount } from '@vue/test-utils';
import VueRouter from 'vue-router';
import vuetify from 'vuetify';
import Vue from 'vue';
import App from './App.vue';
import NavBar from './shared/components/NavBar.vue';

describe('App Test', () => {
  let wrapper: ReturnType<typeof shallowMount>;

  beforeAll(() => {
    Vue.use(vuetify);
    Vue.use(VueRouter);
  });

  beforeEach(() => {
    wrapper = shallowMount(App);
  });

  it('is a Vue instance', () => {
    expect(wrapper.isVueInstance()).toBeTruthy();
  });

  it('contains a NavBar', () => {
    expect(wrapper.contains(NavBar)).toBeTruthy();
  });
});
