import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { ToastrService } from 'ngx-toastr';

import { PayrollListItem } from '@payroll/payroll-list/state/payroll-list.model';
import { PayrollListStore } from '@payroll/payroll-list/state/payroll-list.store';
import { EnvService } from '@shared/env.service';
import { ListResponse, mapListResponseToData } from '@shared/list-response';

@Injectable({ providedIn: 'root' })
export class PayrollListService {
  readonly apiRootUrl: string;

  constructor(
    envService: EnvService,
    private readonly http: HttpClient,
    private readonly store: PayrollListStore,
    private toastr: ToastrService,
  ) {
    this.apiRootUrl = envService.apiRootUrl;
  }

  getPayrolls(department: string): void {
    this.store.setLoading(true);

    let params = new HttpParams();
    params = params.append('Department', department);
    params = params.append('Count', '10');

    this.http
      .get<ListResponse<PayrollListItem>>(
        `${this.apiRootUrl}/departments/payrolls`,
        {
          params,
        },
      )
      .pipe(mapListResponseToData())
      .subscribe({
        error: () => this.toastr.error('Could not load payrolls'),
        next: (response) => this.store.set(response),
        complete: () => this.store.setLoading(false),
      });
  }
}
