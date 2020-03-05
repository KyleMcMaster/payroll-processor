import { Component, OnInit, OnDestroy, Inject } from '@angular/core';
import { Employee } from './state/employee-model';
import { throwError } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.scss'],
})
export class EmployeeListComponent implements OnInit, OnDestroy {
  // employees = of<Employee[]>();
  employeeList: Employee[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit() {
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
      .subscribe(result => (this.employeeList = result));
  }

  ngOnDestroy() {}

  add() {}

  remove(id: string) {
    this.employeeList = this.employeeList.filter(e => e.id !== id);
  }
}
