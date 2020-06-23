import { Injectable } from '@angular/core';
import { Query } from '@datorama/akita';
import { Employee } from './employee.model';
import { EmployeeStore } from './employee.store';

@Injectable({ providedIn: 'root' })
export class EmployeeQuery extends Query<Employee> {
  constructor(protected store: EmployeeStore) {
    super(store);
  }
}
