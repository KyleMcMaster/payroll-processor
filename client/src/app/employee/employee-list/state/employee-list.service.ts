import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EnvService } from 'src/app/shared/env.service';
import { Employee, EmployeeUpdate } from './employee-list.model';
import { EmployeeListStore } from './employee-list.store';

@Injectable({ providedIn: 'root' })
export class EmployeeListService {
  private readonly functionsRootUrl: string;

  constructor(
    private http: HttpClient,
    envService: EnvService,
    private store: EmployeeListStore,
  ) {
    this.functionsRootUrl = envService.functionsRootUrl;
  }

  getEmployees() {
    this.store.setLoading(true);
    return this.http
      .get<Employee[]>(`${this.functionsRootUrl}/Employees`)
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

  updateEmployeeStatus(employee: Employee) {
    this.store.setLoading(true);

    const status = employee.status === 'Enabled' ? 'Disabled' : 'Enabled';

    const employeeUpdate: EmployeeUpdate = {
      department: employee.department,
      status,
    };

    return this.http
      .put<Employee>(
        `${this.functionsRootUrl}/Employees/${employee.id}/status`,
        employeeUpdate,
      )
      .pipe(
        catchError((err) => {
          console.log(`Could not update employee ${employee.id}`);
          return throwError(err);
        }),
      )
      .subscribe({
        next: (detail) => this.store.upsert(detail.id, detail),
        complete: () => this.store.setLoading(false),
      });
  }
}
