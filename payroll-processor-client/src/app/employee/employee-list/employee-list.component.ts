import { Component, OnDestroy, OnInit } from '@angular/core';
import { of } from 'rxjs';

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
  faSkull = faSkull;
  faSmileBeam = faSmileBeam;
  employees = of<Employee[]>();

  constructor(
    private employeeListService: EmployeeListService,
    private employeeListQuery: EmployeeListQuery,
  ) {}

  ngOnInit() {
    this.employeeListService.getEmployees().subscribe();
    this.employees = this.employeeListQuery.selectAll();
  }

  ngOnDestroy() {}

  add() {}

  remove(id: string) {
    // this.dataService.removeEmployee(id);
  }
}
