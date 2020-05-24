
export function isAuthorizationEnabled(): boolean {
  return process.env.VUE_APP_AUTHORIZATION !== 'disabled';
}
