import { Injectable } from '@angular/core';
import { QueryEntity } from '@datorama/akita';
import { EmployeeListItem } from './employee-list.model';
import { EmployeeListItemState, EmployeeListStore } from './employee-list.store';

@Injectable({ providedIn: 'root' })
export class EmployeeListQuery extends QueryEntity<
EmployeeListItemState,
EmployeeListItem
> {
  constructor(protected store: EmployeeListStore) {
    super(store);
  }
}
