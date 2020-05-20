import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { EnvService } from 'src/app/shared/env.service';
import {
  PayrollCreate,
  PayrollList,
} from '../payroll-list/state/payroll-list.model';
import { PayrollListStore } from '../payroll-list/state/payroll-list.store';

@Injectable({ providedIn: 'root' })
export class PayrollCreateService {
  private readonly apiRootUrl: string;

  constructor(
    private http: HttpClient,
    private store: PayrollListStore,
    envService: EnvService,
  ) {
    this.apiRootUrl = envService.apiRootUrl;
  }

  createPayroll(payroll: PayrollCreate) {
    this.store.setLoading(true);
    return this.http
      .post<PayrollList>(`${this.apiRootUrl}/employees/payrolls`, payroll)
      .pipe(
        tap((detail) => {
          this.store.upsert(detail.id, detail);
        }),
        catchError((err) => {
          console.log('Could not create payroll');
          return throwError(err);
        }),
      )
      .subscribe();
  }
}
