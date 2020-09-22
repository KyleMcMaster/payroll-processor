import { Component, Input } from '@angular/core';

import { EmployeePayroll } from '@employee/employee-detail/state/employee-detail.model';

@Component({
  selector: 'app-employee-payroll-list',
  template: ` <h5>Recent Payrolls</h5>
    <ul class="list-group">
      <li
        *ngFor="let payroll of payrolls | slice: 0:8"
        class="list-group-item d-flex justify-content-between border-light"
      >
        <span> {{ payroll.payrollPeriod }} </span>
        <span> {{ payroll.checkDate | date }} </span>
        <span class="badge-pill badge-success">
          {{ payroll.grossPayroll | currency }}
        </span>
      </li>
    </ul>`,
  styles: ['li { background-color: var(--base-color);}'],
})
export class EmployeePayrollListComponent {
  @Input()
  payrolls: EmployeePayroll[];

  constructor() {}
}
