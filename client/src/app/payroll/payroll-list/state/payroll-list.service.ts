import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EnvService } from 'src/app/shared/env.service';
import {
  ListResponse,
  mapListResponseToData,
} from 'src/app/shared/list-response';
import { PayrollList } from './payroll-list.model';
import { PayrollListStore } from './payroll-list.store';

@Injectable({ providedIn: 'root' })
export class PayrollListService {
  readonly apiRootUrl: string;

  constructor(
    envService: EnvService,
    private http: HttpClient,
    protected store: PayrollListStore,
  ) {
    this.apiRootUrl = envService.apiRootUrl;
  }

  getPayrolls(department: string) {
    this.store.setLoading(true);

    let params = new HttpParams();
    params = params.append('Department', department);

    return this.http
      .get<ListResponse<PayrollList>>(
        `${this.apiRootUrl}/departments/payrolls`,
        {
          params,
        },
      )
      .pipe(
        catchError((err) => {
          this.store.setError({
            message: 'Could not load payrolls',
          });
          return of([]);
        }),
        mapListResponseToData(),
      )
      .subscribe({
        next: (response) => this.store.set(response),
        complete: () => this.store.setLoading(false),
      });
  }
}
