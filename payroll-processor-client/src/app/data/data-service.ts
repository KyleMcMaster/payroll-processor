import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { EnvService } from '../shared/env.service';
import { Employee } from './state/employee-model';
import { Payroll } from './state/payroll-model';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  private employees: Employee[] = [];
  private payroll: Payroll[] = [];

  private readonly apiUrl: string;

  constructor(private http: HttpClient, envService: EnvService) {
    this.apiUrl = envService.apiRootUrl;

    this.loadData();
  }

  private loadData() {
    this.http
      .get<Employee[]>(`${this.apiUrl}/EmployeesGetTrigger`)
      .pipe(
        catchError((err) => {
          console.log('Could not fetch employees');
          return throwError(err);
        }),
      )
      .subscribe((result) => (this.employees = result));

    this.http
      .get<Payroll[]>(`${this.apiUrl}/PayrollGetTrigger`)
      .pipe(
        catchError((err) => {
          console.log('Could not fetch payroll');
          return throwError(err);
        }),
      )
      .subscribe((result) => (this.payroll = result));
  }

  getEmployees(): Employee[] {
    return this.employees;
  }

  getPayroll(): Payroll[] {
    return this.payroll;
  }

  removeEmployee(id: string) {
    const employee = this.employees.find((e) => e.id === id);

    if (!employee) {
      return;
    }

    employee.status = 'DISABLED';

    this.payroll
      .filter((p) => p.employeeId === id)
      .filter((payroll) => !!this.payroll.find((p) => p.id === payroll.id))
      .forEach((payroll) => (payroll.employeeStatus = 'DISABLED'));
  }
}
