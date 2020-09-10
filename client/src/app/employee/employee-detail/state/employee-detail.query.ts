import { Injectable } from '@angular/core';

import { Query } from '@datorama/akita';

import { EmployeeDetail } from '@employee/employee-detail/state/employee-detail.model';
import { EmployeeDetailStore } from '@employee/employee-detail/state/employee-detail.store';

@Injectable({ providedIn: 'root' })
export class EmployeeDetailQuery extends Query<EmployeeDetail> {
  constructor(protected store: EmployeeDetailStore) {
    super(store);
  }
}
