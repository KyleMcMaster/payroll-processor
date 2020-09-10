import { Component } from '@angular/core';
import { EmployeeDetailQuery } from '@employee/employee-detail/state/employee-detail.query';
import { EmployeeDetailService } from '@employee/employee-detail/state/employee-detail.service';
import { EmployeeListItem } from '@employee/employee-list/state/employee-list.model';
import { EmployeeListQuery } from '@employee/employee-list/state/employee-list.query';
import { EmployeeListService } from '@employee/employee-list/state/employee-list.service';
import { faPlus } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.scss'],
})
export class EmployeeComponent {
  readonly faPlus = faPlus;
  readonly employee = this.detailQuery.select();
  readonly employees = this.listQuery.selectAll();
  uiState = '';

  constructor(
    private detailQuery: EmployeeDetailQuery,
    private detailService: EmployeeDetailService,
    private listService: EmployeeListService,
    private listQuery: EmployeeListQuery,
  ) {
    this.listService.getEmployees();
  }

  onCreate() {
    this.uiState = 'create';
  }

  onSelectEmployee(employee: EmployeeListItem) {
    this.detailService.getEmployee(employee.id);
    this.uiState = 'detail';
  }
}
