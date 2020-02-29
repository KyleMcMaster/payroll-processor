import { Component, OnInit, OnDestroy } from '@angular/core';
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
    const url = '';
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*',
      }),
    };
    this.employees = this.http.get<Employee[]>(url, httpOptions).pipe(
      tap(t => console.log(t)),
      catchError(err => {
        console.log('Could not fetch employees');
        return throwError(err);
      }),
    );
  }
  ngOnDestroy() {}
}
