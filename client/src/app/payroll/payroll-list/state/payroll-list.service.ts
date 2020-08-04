import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { PayrollListItem } from './payroll-list.model';
import { PayrollListStore } from './payroll-list.store';

import { ToastrService } from 'ngx-toastr';
import { EnvService } from 'src/app/shared/env.service';
import { ListResponse, mapListResponseToData } from 'src/app/shared/list-response';

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
