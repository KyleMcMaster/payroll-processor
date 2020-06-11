import { Component } from '@angular/core';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { Observable } from 'rxjs';
import { Employee } from './employee-detail/state/employee.model';
import { EmployeeQuery } from './employee-detail/state/employee.query';
import { EmployeeService } from './employee-detail/state/employee.service';
import { EmployeeListItem } from './employee-list/state/employee-list.model';
import { EmployeeListQuery } from './employee-list/state/employee-list.query';
import { EmployeeListService } from './employee-list/state/employee-list.service';

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.scss'],
})
export class EmployeeComponent {
  readonly faPlus = faPlus;
  employees: Observable<EmployeeListItem[]>;
  employee: Observable<Employee>;
  uiState = '';

  constructor(
    private employeeQuery: EmployeeQuery,
    private employeeService: EmployeeService,
    private employeeListService: EmployeeListService,
    private employeeListQuery: EmployeeListQuery,
  ) {
    this.employee = this.employeeQuery.select((state) => state as Employee);
    this.employees = this.employeeListQuery.selectAll();
    this.employeeListService.getEmployees();
  }

  setCreate() {
    this.uiState = 'create';
  }

  setActive(employee: EmployeeListItem) {
    this.employeeService.getEmployee(employee.id);
    this.uiState = 'detail';
  }
}
