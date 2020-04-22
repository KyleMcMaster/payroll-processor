import { Injectable } from '@angular/core';
import { Environment } from '../../environments/environment-types';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EnvService {
  get env(): Environment {
    return environment;
  }

  get apiRootUrl(): string {
    return `${this.env}/api`;
  }
}
