import { Injectable } from '@angular/core';
import { Query } from '@datorama/akita';
import {
  EmployeeDetailState,
  EmployeeDetailStore,
} from './employee-detail.store';

@Injectable({ providedIn: 'root' })
export class EmployeeDetailQuery extends Query<EmployeeDetailState> {
  constructor(protected store: EmployeeDetailStore) {
    super(store);
  }
}
