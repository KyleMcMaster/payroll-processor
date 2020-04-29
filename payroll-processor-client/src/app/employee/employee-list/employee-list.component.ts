import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

import { faSkull, faSmileBeam } from '@fortawesome/free-solid-svg-icons';

import { Employee, EmployeeUpdate } from './state/employee-list.model';
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

  updateEmployeeStatus(id: string) {
    let employee: Employee;

    this.employees
      .pipe(
        tap((employees) => {
          return (employee = employees.find((emp) => emp.id === id));
        }),
      )
      .subscribe();

    const status = employee.status === 'ACTIVE' ? 'DISABLED' : 'ACTIVE';

    const employeeUpdate: EmployeeUpdate = {
      department: employee.department,
      status,
    };

    this.service.updateEmployeeStatus(id, employeeUpdate);
  }
}
