import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { ToastrService } from 'ngx-toastr';

import { EmployeeDetail, EmployeeUpdate } from '@employee/employee-detail/state/employee-detail.model';
import { EmployeeDetailStore } from '@employee/employee-detail/state/employee-detail.store';
import { EnvService } from '@shared/env.service';

@Injectable({ providedIn: 'root' })
export class EmployeeDetailService {
  readonly apiRootUrl: string;

  constructor(
    envService: EnvService,
    private http: HttpClient,
    private store: EmployeeDetailStore,
    private toastr: ToastrService,
  ) {
    this.apiRootUrl = envService.apiRootUrl;
  }

  getEmployee(id: string) {
    this.store.setLoading(true);

    this.http
      .get<EmployeeDetail>(`${this.apiRootUrl}/Employees/${id}`)
      .subscribe({
        error: () => this.toastr.error(`Could not get employee ${id}`),
        next: (detail) => this.store.update(detail),
        complete: () => this.store.setLoading(false),
      });
  }

  update(employeeUpdate: EmployeeUpdate): void {
    this.store.setLoading(true);

    this.http
      .put<EmployeeDetail>(`${this.apiRootUrl}/Employees`, employeeUpdate)
      .subscribe({
        error: () =>
          this.toastr.error(`Could not update employee ${employeeUpdate.id}`),
        next: (detail) => {
          this.toastr.show('Employee sucessfully updated!');
          this.store.update(detail);
        },
        complete: () => this.store.setLoading(false),
      });
  }
}
