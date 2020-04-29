import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { EnvService } from '../shared/env.service';

import { Payroll } from './state/payroll-model';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  private payroll: Payroll[] = [];

  private readonly apiUrl: string;

  constructor(private http: HttpClient, envService: EnvService) {
    this.apiUrl = envService.apiRootUrl;

    this.loadData();
  }

  private loadData() {
    this.http
      .get<Payroll[]>(`${this.apiUrl}/Payrolls`)
      .pipe(
        catchError((err) => {
          console.log('Could not fetch payrolls');
          return throwError(err);
        }),
      )
      .subscribe((result) => (this.payroll = result));
  }

  getPayroll(): Payroll[] {
    return this.payroll;
  }

  // removeEmployee(id: string) {
  //   const employee = this.employees.find((e) => e.id === id);

  //   if (!employee) {
  //     return;
  //   }

  //   employee.status = 'DISABLED';

  //   this.payroll
  //     .filter((p) => p.employeeId === id)
  //     .filter((payroll) => !!this.payroll.find((p) => p.id === payroll.id))
  //     .forEach((payroll) => (payroll.employeeStatus = 'DISABLED'));
  // }
}
