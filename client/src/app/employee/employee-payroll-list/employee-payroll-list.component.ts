import { Component, Input, OnInit } from '@angular/core';

import { EmployeePayroll } from '@employee/employee-detail/state/employee.model';

@Component({
  selector: 'app-employee-payroll-list',
  templateUrl: './employee-payroll-list.component.html',
  styleUrls: ['./employee-payroll-list.component.scss'],
})
export class EmployeePayrollListComponent implements OnInit {
  @Input()
  payrolls: Array<EmployeePayroll>;

  constructor() {}

  ngOnInit(): void {}
}
