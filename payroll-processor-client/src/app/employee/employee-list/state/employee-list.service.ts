import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { Employee } from './employee-list.model';

import { EnvService } from 'src/app/shared/env.service';

@Injectable({ providedIn: 'root' })
export class EmployeeListService {
  private readonly apiUrl: string;
  private employees: Employee[];

  constructor(private http: HttpClient, envService: EnvService) {
    this.apiUrl = envService.apiRootUrl;

    this.http
      .get<Employee[]>(`${this.apiUrl}/Employees`)
      .pipe(
        catchError((err) => {
          console.log('Could not fetch employees');
          return throwError(err);
        }),
      )
      .subscribe((result) => (this.employees = result));
  }

  getEmployees(): Employee[] {
    console.log(this.employees);
    return this.employees;
  }
}
