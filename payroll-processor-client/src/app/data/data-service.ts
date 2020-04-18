import { Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Employee } from './state/models';
import { catchError } from 'rxjs/operators';
import { throwError, of, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  private employees: Employee[] = [];

  constructor(private http: HttpClient) {
    this.loadData();
  }

  private loadData() {
    const url =
      'https://nitro-km-payroll-processor.azurewebsites.net/api/EmployeesGetTrigger';

    this.http
      .get<Employee[]>(url)
      .pipe(
        catchError(err => {
          console.log('Could not fetch employees');
          return throwError(err);
        }),
      )
      .subscribe(result => (this.employees = result));
  }

  getEmployees(): Employee[] {
    return this.employees;
  }

  removeEmployee(id: string) {
    this.employees.find(e => e.id === id).status = 'DISABLED';
  }
}
