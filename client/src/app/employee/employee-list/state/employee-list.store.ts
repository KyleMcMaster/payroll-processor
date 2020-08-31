import { Injectable } from '@angular/core';

import { EntityState, EntityStore, StoreConfig } from '@datorama/akita';

import { createInitialState, EmployeeListItem } from '@employee/employee-list/state/employee-list.model';

export interface EmployeeListItemState extends EntityState<EmployeeListItem> {}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'employees' })
export class EmployeeListStore extends EntityStore<EmployeeListItemState> {
  constructor() {
    super(createInitialState());
  }
}
