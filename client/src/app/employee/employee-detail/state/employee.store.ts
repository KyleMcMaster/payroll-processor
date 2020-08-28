import { Injectable } from '@angular/core';

import { Store, StoreConfig } from '@datorama/akita';

import { createInitialState, EmployeeDetail } from './employee.model';

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'employee' })
export class EmployeeStore extends Store<EmployeeDetail> {
  constructor() {
    super(createInitialState());
  }
}
