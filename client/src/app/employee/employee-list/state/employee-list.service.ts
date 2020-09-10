import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { ToastrService } from 'ngx-toastr';

import { EmployeeDetail } from '@employee/employee-detail/state/employee-detail.model';
import { EmployeeCreate, EmployeeListItem } from '@employee/employee-list/state/employee-list.model';
import { EmployeeListStore } from '@employee/employee-list/state/employee-list.store';
import { EnvService } from '@shared/env.service';
import { ListResponse, mapListResponseToData } from '@shared/list-response';

@Injectable({ providedIn: 'root' })
export class EmployeeListService {
  private readonly apiRootUrl: string;

  constructor(
    private http: HttpClient,
    envService: EnvService,
    private store: EmployeeListStore,
    private toastr: ToastrService,
  ) {
    this.apiRootUrl = envService.apiRootUrl;
  }

  getEmployees(): void {
    this.store.setLoading(true);
    this.http
      .get<ListResponse<EmployeeListItem>>(`${this.apiRootUrl}/Employees`)
      .pipe(
        catchError((err) => {
          this.store.setError({
            message: 'Could not load employees',
          });
          return of([]);
        }),
        mapListResponseToData(),
      )
      .subscribe({
        next: (response) => this.store.set(response),
        complete: () => this.store.setLoading(false),
      });
  }

  createEmployee(employee: EmployeeCreate): void {
    this.store.setLoading(true);
    this.http
      .post<EmployeeDetail>(`${this.apiRootUrl}/employees`, employee)
      .subscribe({
        error: () => this.toastr.error(`Could not create employee`),
        next: (detail) => {
          this.toastr.show('Employee sucessfully created!');
          this.store.upsert(detail.id, detail);
        },
        complete: () => this.store.setLoading(false),
      });
  }
}
