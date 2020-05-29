import { Component } from '@angular/core';
import { faLock, faUnlock } from '@fortawesome/free-solid-svg-icons';
import { Observable } from 'rxjs';
import { Employee } from './employee-detail/state/employee-detail.model';
import { EmployeeDetailQuery } from './employee-detail/state/employee-detail.query';
import { EmployeeDetailService } from './employee-detail/state/employee-detail.service';
import { EmployeeList } from './employee-list/state/employee-list.model';
import { EmployeeListQuery } from './employee-list/state/employee-list.query';
import { EmployeeListService } from './employee-list/state/employee-list.service';

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.scss'],
})
export class EmployeeComponent {
  readonly faLock = faLock;
  readonly faUnlock = faUnlock;
  employees: Observable<EmployeeList[]>;
  employee: Observable<Employee>;
  uiState = '';

  constructor(
    private employeeDetailQuery: EmployeeDetailQuery,
    private employeeDetailService: EmployeeDetailService,
    private employeeListService: EmployeeListService,
    private employeeListQuery: EmployeeListQuery,
  ) {
    this.employee = this.employeeDetailQuery.select(
      (state) => state as Employee,
    );

    this.employees = this.employeeListQuery.selectAll();
    this.employeeListService.getEmployees();
  }

  setCreate() {
    this.uiState = 'create';
  }

  setActive(employee: EmployeeList) {
    this.employeeDetailService.getEmployee(employee.id);
    this.uiState = 'detail';
  }
}
