import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import { Employee, EmployeeCreate } from '../employee-list/state/employee-list.model';
import { EmployeeListStore } from '../employee-list/state/employee-list.store';

import { EnvService } from 'src/app/shared/env.service';

@Injectable({ providedIn: 'root' })
export class EmployeeCreateService {
  private readonly apiUrl: string;

  constructor(
    private http: HttpClient,
    private employeeListStore: EmployeeListStore,
    // private router: Router,
    envService: EnvService,
  ) {
    this.apiUrl = envService.apiRootUrl;
  }

  createEmployee(employee: EmployeeCreate) {
    // console.log(employee);
    return this.http
      .post<Employee>(`${this.apiUrl}/employees`, employee)
      .pipe(
        tap((detail) => {
          this.employeeListStore.upsert(detail.id, detail);
        }),
        // map((detail) => this.router.navigate(['/employees'])), // TODO: route to list?
        catchError((err) => {
          console.log('Could not create employee');
          return throwError(err);
        }),
      )
      .subscribe();
  }
}
