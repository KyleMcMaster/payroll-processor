import { Injectable } from '@angular/core';
import { QueryEntity } from '@datorama/akita';
import { PayrollListState, PayrollListStore } from './payroll-list.store';

@Injectable({ providedIn: 'root' })
export class PayrollListQuery extends QueryEntity<PayrollListState> {
  constructor(protected store: PayrollListStore) {
    super(store);
  }
}
