import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EMPTY } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { PayrollList } from './payroll-list.model';
import { PayrollListStore } from './payroll-list.store';

import { EnvService } from 'src/app/shared/env.service';
import { ListResponse, mapListResponseToData } from 'src/app/shared/list-response';

@Injectable({ providedIn: 'root' })
export class PayrollListService {
  readonly apiRootUrl: string;

  constructor(
    envService: EnvService,
    private readonly http: HttpClient,
    private readonly store: PayrollListStore,
  ) {
    this.apiRootUrl = envService.apiRootUrl;
  }

  getPayrolls(department: string) {
    this.store.setLoading(true);

    let params = new HttpParams();
    params = params.append('Department', department);
    params = params.append('Count', '10');

    return this.http
      .get<ListResponse<PayrollList>>(
        `${this.apiRootUrl}/departments/payrolls`,
        {
          params,
        },
      )
      .pipe(
        mapListResponseToData(),
        catchError((err) => {
          this.store.setError({
            message: 'Could not load payrolls',
          });
          return EMPTY;
        }),
      )
      .subscribe({
        next: (response) => this.store.set(response),
        complete: () => this.store.setLoading(false),
      });
  }
}
