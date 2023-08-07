import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'NightMarket',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44326/',
    redirectUri: baseUrl,
    clientId: 'NightMarket_App',
    responseType: 'code',
    scope: 'offline_access NightMarket',
    requireHttps: true,
  },
  apis: {
    default: {
      url: 'https://localhost:44399',
      rootNamespace: 'NightMarket',
    },
  },
} as Environment;
