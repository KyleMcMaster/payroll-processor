import { Injectable } from '@angular/core';
import { EntityState, EntityStore, StoreConfig } from '@datorama/akita';
import { createInitialState, EmployeeList } from './employee-list.model';

export interface EmployeeListState extends EntityState<EmployeeList> {}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'employees' })
export class EmployeeListStore extends EntityStore<EmployeeListState> {
  constructor() {
    super(createInitialState());
  }
}
