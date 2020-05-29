import { Injectable } from '@angular/core';
import { QueryEntity } from '@datorama/akita';
import { EmployeeList } from './employee-list.model';
import { EmployeeListState, EmployeeListStore } from './employee-list.store';

@Injectable({ providedIn: 'root' })
export class EmployeeListQuery extends QueryEntity<
  EmployeeListState,
  EmployeeList
> {
  constructor(protected store: EmployeeListStore) {
    super(store);
  }
}
