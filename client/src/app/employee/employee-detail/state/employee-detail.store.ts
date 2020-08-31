import { Injectable } from '@angular/core';

import { Store, StoreConfig } from '@datorama/akita';

import { createInitialState, EmployeeDetail } from '@employee/employee-detail/state/employee-detail.model';

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'employee' })
export class EmployeeDetailStore extends Store<EmployeeDetail> {
  constructor() {
    super(createInitialState());
  }
}
