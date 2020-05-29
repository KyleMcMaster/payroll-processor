import { Injectable } from '@angular/core';
import { EntityState, Store, StoreConfig } from '@datorama/akita';
import { createInitialState, Employee } from './employee-detail.model';

export interface EmployeeDetailState extends EntityState<Employee> {}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'employee-detail' })
export class EmployeeDetailStore extends Store<EmployeeDetailState> {
  constructor() {
    super(createInitialState());
  }
}
