import { Component, Input } from '@angular/core';

import { EmployeePayroll } from '@employee/employee-detail/state/employee.model';

@Component({
  selector: 'app-employee-payroll-list',
  template: ` <h5>Recent Payrolls</h5>
    <div class="row">
      <div class="col-6">
        <ul class="list-group">
          <li
            *ngFor="let payroll of payrolls | slice: 0:9"
            class="list-group-item d-flex justify-content-between payroll-list-item"
          >
            <span> {{ payroll.payrollPeriod }} </span>
            <span> {{ payroll.checkDate | date }} </span>
            <span class="badge-pill badge-success">
              {{ payroll.grossPayroll | currency }}
            </span>
          </li>
        </ul>
      </div>
    </div>`,
  styles: [
    `
      .payroll-list-item {
        background-color: #000;
      }
    `,
  ],
})
export class EmployeePayrollListComponent {
  @Input()
  payrolls: EmployeePayroll[];

  constructor() {}
}
