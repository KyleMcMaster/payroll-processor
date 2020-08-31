import { Injectable } from '@angular/core';

import { Query } from '@datorama/akita';

import { EmployeeDetail } from '@employee/employee-detail/state/employee.model';
import { EmployeeStore } from '@employee/employee-detail/state/employee.store';

@Injectable({ providedIn: 'root' })
export class EmployeeQuery extends Query<EmployeeDetail> {
  constructor(protected store: EmployeeStore) {
    super(store);
  }
}
