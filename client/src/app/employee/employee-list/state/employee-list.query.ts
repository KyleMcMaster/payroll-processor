import { Injectable } from '@angular/core';

import { QueryEntity } from '@datorama/akita';

import { Employee } from './employee-list.model';
import { EmployeeListState, EmployeeListStore } from './employee-list.store';

@Injectable({ providedIn: 'root' })
export class EmployeeListQuery extends QueryEntity<
  EmployeeListState,
  Employee
> {
  constructor(protected store: EmployeeListStore) {
    super(store);
  }
}
