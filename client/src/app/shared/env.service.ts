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

  get functionsRootUrl(): string {
    return `${this.env.functionsDomain}/api`;
  }

  get apiRootUrl(): string {
    return `${this.env.apiDomain}/api`;
  }
}
