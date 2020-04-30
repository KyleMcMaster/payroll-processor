import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { Employee, EmployeeUpdate } from './employee-list.model';
import { EmployeeListStore } from './employee-list.store';

import { EnvService } from 'src/app/shared/env.service';

@Injectable({ providedIn: 'root' })
export class EmployeeListService {
  private readonly apiUrl: string;

  constructor(
    private http: HttpClient,
    envService: EnvService,
    private store: EmployeeListStore,
  ) {
    this.apiUrl = envService.apiRootUrl;
  }

  getEmployees() {
    this.store.setLoading(true);
    return this.http
      .get<Employee[]>(`${this.apiUrl}/Employees`)
      .pipe(
        catchError((err) => {
          this.store.setError({
            message: 'Could not load employees',
          });
          return of([]);
        }),
      )
      .subscribe({
        next: (employees) => this.store.set(employees),
        complete: () => this.store.setLoading(false),
      });
  }

  updateEmployeeStatus(id: string, employee: EmployeeUpdate) {
    this.store.setLoading(true);
    return this.http
      .put<Employee>(`${this.apiUrl}/Employees/${id}/status`, employee)
      .pipe(
        catchError((err) => {
          console.log('Could not update employee');
          return throwError(err);
        }),
      )
      .subscribe({
        next: (detail) => this.store.upsert(detail.id, detail),
        complete: () => this.store.setLoading(false),
      });
  }
}
