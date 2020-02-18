import { Component, OnInit } from '@angular/core';
import {
  employeePayrolls,
  EmployeePayroll,
} from './state/employee-payroll-model';

@Component({
  selector: 'app-employee-payroll-list',
  templateUrl: './employee-payroll-list.component.html',
  styleUrls: ['./employee-payroll-list.component.scss'],
})
export class EmployeePayrollListComponent implements OnInit {
  employeePayrolls: EmployeePayroll[];

  constructor() {
    this.employeePayrolls = employeePayrolls;
  }

  ngOnInit() {}
}
