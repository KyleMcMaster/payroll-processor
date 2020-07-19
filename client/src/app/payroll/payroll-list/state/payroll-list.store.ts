import { Injectable } from '@angular/core';

import { EntityState, EntityStore, StoreConfig } from '@datorama/akita';

import { createInitialState, PayrollListItem } from './payroll-list.model';

export interface PayrollListState extends EntityState<PayrollListItem> {}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'payrolls' })
export class PayrollListStore extends EntityStore<PayrollListState> {
  constructor() {
    super(createInitialState());
  }
}
