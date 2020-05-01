import { getInstance } from './index';
import { Route } from 'vue-router';

export const pageGuard = (to: Route, from: Route, next: Function) => {
  const authService = getInstance();

  const fn = () => {
    if (authService.isAuthenticated) {
      return next();
    } else {
      authService.loginWithRedirect({ appState: { targetUrl: to.fullPath } });
    }
  };

  if (!authService.loading) {
    return fn();
  }

  authService.$watch('loading', (loading: boolean) => {
    if (!loading) {
      return fn();
    }
  });
};
