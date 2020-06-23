import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EnvService } from 'src/app/shared/env.service';
import { Employee, EmployeeUpdate } from './employee.model';
import { EmployeeStore } from './employee.store';

@Injectable({ providedIn: 'root' })
export class EmployeeService {
  readonly apiRootUrl: string;

  constructor(
    envService: EnvService,
    private http: HttpClient,
    private store: EmployeeStore,
    private toastr: ToastrService,
  ) {
    this.apiRootUrl = envService.apiRootUrl;
  }

  getEmployee(id: string) {
    this.store.setLoading(true);

    this.http.get<Employee>(`${this.apiRootUrl}/Employees/${id}`).subscribe({
      error: () => this.toastr.error(`Could not get employee ${id}`),
      next: (detail) => this.store.update(detail),
      complete: () => this.store.setLoading(false),
    });
  }

  updateEmployeeStatus(employee: Employee) {
    this.store.setLoading(true);

    const status = employee.status === 'Enabled' ? 'Disabled' : 'Enabled';

    const employeeUpdate: EmployeeUpdate = {
      ...employee,
      status,
    };

    return this.http
      .put<Employee>(`${this.apiRootUrl}/Employees`, employeeUpdate)
      .pipe(
        catchError((err) => {
          console.log(`Could not update employee ${employee.id}`);
          return throwError(err);
        }),
      )
      .subscribe({
        next: (detail) => {
          this.toastr.show('Employee sucessfully updated!');
          this.store.update(detail);
        },
        complete: () => this.store.setLoading(false),
      });
  }
}
