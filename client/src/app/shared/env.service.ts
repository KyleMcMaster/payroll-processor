import { Injectable } from '@angular/core';

import { environment } from '../../environments/environment';
import { Environment } from '../../environments/environment-types';

@Injectable({
  providedIn: 'root',
})
export class EnvService {
  get env(): Environment {
    return environment;
  }

  get authenticationUrl(): string {
    return `${this.env.authenticationDomain}&client_id=${this.env.clientId}`;
    //`https://github.com/login/oauth/authorize?scope=user&client_id=${client_id}&redirect_uri=${redirect_uri}`
  }

  get functionsRootUrl(): string {
    return `${this.env.functionsDomain}/api`;
  }

  get apiRootUrl(): string {
    return `${this.env.apiDomain}/api/v1`;
  }
}
