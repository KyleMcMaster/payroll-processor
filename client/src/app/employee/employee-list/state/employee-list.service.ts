import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EnvService } from '../../../shared/env.service';
import { Employee } from './employee-list.model';
import { EmployeeListStore } from './employee-list.store';

@Injectable({ providedIn: 'root' })
export class EmployeeListService {
  private readonly apiUrl: string;

  constructor(
    private http: HttpClient,
    envService: EnvService,
    private store: EmployeeListStore,
  ) {
    this.apiUrl = envService.functionsRootUrl;
  }

  getEmployees() {
    this.store.setLoading(true);
    return this.http
      .get<Employee[]>(`${this.apiUrl}/employees`)
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
}
