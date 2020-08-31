import { Component } from '@angular/core';

import { faPlus } from '@fortawesome/free-solid-svg-icons';

import { EmployeeQuery } from '@employee/employee-detail/state/employee.query';
import { EmployeeService } from '@employee/employee-detail/state/employee.service';
import { EmployeeListItem } from '@employee/employee-list/state/employee-list.model';
import { EmployeeListQuery } from '@employee/employee-list/state/employee-list.query';
import { EmployeeListService } from '@employee/employee-list/state/employee-list.service';

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.scss'],
})
export class EmployeeComponent {
  readonly faPlus = faPlus;
  readonly employees = this.employeeListQuery.selectAll();
  readonly employee = this.employeeQuery.select();
  uiState = '';

  constructor(
    private employeeQuery: EmployeeQuery,
    private employeeService: EmployeeService,
    private employeeListService: EmployeeListService,
    private employeeListQuery: EmployeeListQuery,
  ) {
    this.employeeListService.getEmployees();
  }

  onCreate() {
    this.uiState = 'create';
  }

  onSelectEmployee(employee: EmployeeListItem) {
    this.employeeService.getEmployee(employee.id);
    this.uiState = 'detail';
  }
}
