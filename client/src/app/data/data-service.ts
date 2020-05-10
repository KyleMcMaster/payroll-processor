import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EnvService } from '../shared/env.service';
import { Payroll } from './state/payroll-model';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  private payroll: Payroll[] = [];

  private readonly apiRootUrl: string;

  constructor(private http: HttpClient, envService: EnvService) {
    this.apiRootUrl = envService.apiRootUrl;

    this.loadData();
  }

  private loadData() {
    this.http
      .get<Payroll[]>(`${this.apiRootUrl}/payrolls`)
      .pipe(
        catchError((err) => {
          console.log('Could not fetch payrolls');
          return throwError(err);
        }),
      )
      .subscribe((result) => (this.payroll = result));
  }

  getPayroll(): Payroll[] {
    return this.payroll;
  }
}
