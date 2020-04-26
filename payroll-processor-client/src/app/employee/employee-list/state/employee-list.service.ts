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

  constructor(
    private http: HttpClient,
    envService: EnvService,
    private store: EmployeeListStore,
  ) {
    this.apiUrl = envService.apiRootUrl;
  }

  getEmployees(): Observable<void | Employee[]> {
    return this.http.get<Employee[]>(`${this.apiUrl}/Employees`).pipe(
      map((employees) => {
        this.store.set(employees);
        this.store.setLoading(false);
      }),
      catchError((err) => {
        console.log('Could not fetch employees');
        return throwError(err);
      }),
    );
  }
}
