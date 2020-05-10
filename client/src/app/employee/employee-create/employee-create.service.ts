import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { EnvService } from 'src/app/shared/env.service';
import {
  Employee,
  EmployeeCreate,
} from '../employee-list/state/employee-list.model';
import { EmployeeListStore } from '../employee-list/state/employee-list.store';

@Injectable({ providedIn: 'root' })
export class EmployeeCreateService {
  private readonly apiRootUrl: string;

  constructor(
    private http: HttpClient,
    private store: EmployeeListStore,
    envService: EnvService,
  ) {
    this.apiRootUrl = envService.apiRootUrl;
  }

  createEmployee(employee: EmployeeCreate) {
    this.store.setLoading(true);
    return this.http
      .post<Employee>(`${this.apiRootUrl}/employees`, employee)
      .pipe(
        tap((detail) => {
          this.store.upsert(detail.id, detail);
        }),
        catchError((err) => {
          console.log('Could not create employee');
          return throwError(err);
        }),
      )
      .subscribe();
  }
}
