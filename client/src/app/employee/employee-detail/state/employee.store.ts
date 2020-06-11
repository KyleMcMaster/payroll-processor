import { Injectable } from '@angular/core';
import { Store, StoreConfig } from '@datorama/akita';
import { createInitialState, Employee } from './employee.model';

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'employee' })
export class EmployeeStore extends Store<Employee> {
  constructor() {
    super(createInitialState());
  }
}
