import { Injectable } from '@angular/core';

import { Store, StoreConfig } from '@datorama/akita';

export interface ResourcesState {
  stats: {
    totalPayrolls: number;
    totalEmployees: number;
  };
}

export function createInitialState(): ResourcesState {
  return {
    stats: {
      totalEmployees: 0,
      totalPayrolls: 0,
    },
  };
}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'resources' })
export class ResourcesStore extends Store<ResourcesState> {
  constructor() {
    super(createInitialState());
  }
}
