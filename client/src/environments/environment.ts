import { Environment } from './environment-types';

export const environment: Environment = {
  production: false,
  // This should come from the launch.json for the API
  functionsDomain: '<TODO>',
  apiDomain: '<TODO>',
  authenticationDomain: '',
  clientId: '',
  clientSecret: '',
};
