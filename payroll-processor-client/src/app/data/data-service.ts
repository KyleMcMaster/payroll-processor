import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Employee } from './state/employee-model';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { Payroll } from './state/payroll-model';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  private employees: Employee[] = [];
  private payroll: Payroll[] = [];

  constructor(private http: HttpClient) {
    this.loadData();
  }

  private loadData() {
    const employeeUrl =
      'https://nitro-km-payroll-processor.azurewebsites.net/api/EmployeesGetTrigger';

    this.http
      .get<Employee[]>(employeeUrl)
      .pipe(
        catchError(err => {
          console.log('Could not fetch employees');
          return throwError(err);
        }),
      )
      .subscribe(result => (this.employees = result));

    const payrollUrl =
      'https://nitro-km-payroll-processor.azurewebsites.net/api/PayrollGetTrigger';

    this.http
      .get<Payroll[]>(payrollUrl)
      .pipe(
        catchError(err => {
          console.log('Could not fetch payroll');
          return throwError(err);
        }),
      )
      .subscribe(result => (this.payroll = result));
  }

  getEmployees(): Employee[] {
    return this.employees;
  }

  getPayroll(): Payroll[] {
    return this.payroll;
  }

  removeEmployee(id: string) {
    this.employees.find(e => e.id === id).status = 'DISABLED';
    this.payroll
      .filter(p => p.employeeId === id)
      .forEach(payroll => {
        this.payroll.find(p => p.id === payroll.id).employeeStatus = 'DISABLED';
      });
  }
}
