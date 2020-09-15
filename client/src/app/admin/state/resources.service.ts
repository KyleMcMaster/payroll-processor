import { Injectable } from '@angular/core';
import { switchMap, tap } from 'rxjs/operators';

import { ToastrService } from 'ngx-toastr';

import { mapErrorResponse } from '@shared/api-error';

import { ResourcesClient } from './resources.client';
import { ResourcesStore } from './resources.store';

@Injectable({ providedIn: 'root' })
export class ResourcesService {
  constructor(
    private readonly store: ResourcesStore,
    private readonly client: ResourcesClient,
    private readonly toasts: ToastrService,
  ) {}

  getStats(): void {
    this.store.setLoading(true);

    this.client.getStats().subscribe({
      next: ({ totalEmployees, totalPayrolls }) =>
        this.store.update((s) => ({
          ...s,
          stats: {
            totalEmployees,
            totalPayrolls,
            elapsedMilliseconds: 0,
          },
        })),
      error: mapErrorResponse(({ id, message }) =>
        this.toasts.error(
          `Could not acquire resource stats: (${id}) ${message.substring(
            0,
            100,
          )}`,
        ),
      ),
      complete: () => this.store.setLoading(false),
    });
  }

  create(employeesCount: number, payrollsMaxCount: number): void {
    this.store.setLoading(true);

    this.client
      .createResources()
      .pipe(
        tap(() => this.toasts.success(`Resources initialized`)),
        switchMap(() =>
          this.client.createData({ employeesCount, payrollsMaxCount }),
        ),
        tap(({ totalEmployees, totalPayrolls, totalMilliseconds }) =>
          this.toasts.success(
            `Created ${totalEmployees} Employees, ${totalPayrolls} Payrolls in ${
              totalMilliseconds / 1000
            } seconds`,
          ),
        ),
        tap(() => this.getStats()),
      )
      .subscribe({
        error: (error) => this.toasts.error(`Could not create data: ${error}`),
        complete: () => this.store.setLoading(false),
      });
  }

  reset(): void {
    this.store.setLoading(true);

    this.client
      .resetResources()
      .pipe(tap(() => this.getStats()))
      .subscribe({
        next: () => this.toasts.success('Resources reset'),
        error: (error) =>
          this.toasts.error(`Resources could not be reset: ${error}`),
        complete: () => this.store.setLoading(false),
      });
  }
}
