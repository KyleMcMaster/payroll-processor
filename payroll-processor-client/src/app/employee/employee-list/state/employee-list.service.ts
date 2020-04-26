import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { Employee } from './employee-list.model';
import { EmployeeListStore } from './employee-list.store';

import { EnvService } from 'src/app/shared/env.service';

@Injectable({ providedIn: 'root' })
export class EmployeeListService {
  private readonly apiUrl: string;
  private employees: Employee[];

  constructor(
    private http: HttpClient,
    envService: EnvService,
    private employeeListStore: EmployeeListStore,
  ) {
    this.apiUrl = envService.apiRootUrl;
  }

  getEmployees(): Observable<void | Employee[]> {
    return this.http.get<Employee[]>(`${this.apiUrl}/Employees`).pipe(
      map((employees) => {
        this.employeeListStore.set(employees);
        this.employeeListStore.setLoading(false);
      }),
      catchError((err) => {
        console.log('Could not fetch employees');
        return throwError(err);
      }),
    );
  }
}
