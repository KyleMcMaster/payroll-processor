import { Injectable } from '@angular/core';

import { EntityState, EntityStore, StoreConfig } from '@datorama/akita';

import { createInitialState, Employee } from './employee-list.model';

export interface EmployeeListState extends EntityState<Employee> {}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'employees' })
export class EmployeeListStore extends EntityStore<EmployeeListState> {
  constructor() {
    super(createInitialState());
  }
}
