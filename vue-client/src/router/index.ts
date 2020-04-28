import Vue from 'vue';
import VueRouter from 'vue-router';
import Home from '../views/Home.vue';
import Revision from '../views/Revision.vue';
import Upload from '../views/Upload.vue';
import Gallery from '../views/Gallery.vue';

Vue.use(VueRouter);

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/user',
    name: 'User',
    component: () => import('../views/User.vue')
  },
  {
    path: '/revision',
    name: 'Revision',
    component: Revision
  },
  {
    path: '/upload',
    name: 'Upload',
    component: Upload
  },
  {
    path: '/gallery/:data',
    name: 'Gallery',
    component: Gallery
  }
];

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
});

export default router;
