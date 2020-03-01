import { Component, OnInit, OnDestroy, Inject } from '@angular/core';
import { Employee } from './state/employee-model';
import { of, throwError } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map, tap } from 'rxjs/operators';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.scss'],
})
export class EmployeeListComponent implements OnInit, OnDestroy {
  employees = of<Employee[]>();

  constructor(private http: HttpClient) {}

  ngOnInit() {
    const url =
      'https://nitro-km-payroll-processor.azurewebsites.net/api/EmployeesGetTrigger';
    this.employees = this.http.get<Employee[]>(url).pipe(
      catchError(err => {
        console.log('Could not fetch employees');
        return throwError(err);
      }),
    );
  }

  ngOnDestroy() {}

  remove(id: string) {}
}
