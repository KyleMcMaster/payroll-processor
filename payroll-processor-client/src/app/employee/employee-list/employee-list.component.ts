import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { faSkull, faSmileBeam } from '@fortawesome/free-solid-svg-icons';

import { Employee } from './state/employee-list.model';
import { EmployeeListQuery } from './state/employee-list.query';
import { EmployeeListService } from './state/employee-list.service';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.scss'],
})
export class EmployeeListComponent implements OnInit, OnDestroy {
  readonly faSkull = faSkull;
  readonly faSmileBeam = faSmileBeam;
  readonly employees: Observable<Employee[]>;

  constructor(
    private service: EmployeeListService,
    private query: EmployeeListQuery,
  ) {
    this.employees = this.query.selectAll();
    this.service.getEmployees();
  }

  ngOnInit() {}

  ngOnDestroy() {}

  updateEmployeeStatus(employee: Employee) {
    this.service.updateEmployeeStatus(employee);
  }
}
