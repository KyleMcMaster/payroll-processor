import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';

import { Query } from '@datorama/akita';
import { NgxSpinnerService } from 'ngx-spinner';

import { ResourcesState, ResourcesStore } from './resources.store';

@Injectable({ providedIn: 'root' })
export class ResourcesQuery extends Query<ResourcesState> {
  readonly stats = this.select().pipe(map(({ stats }) => stats));

  constructor(
    protected store: ResourcesStore,
    private spinner: NgxSpinnerService,
  ) {
    super(store);

    this.selectLoading().subscribe({
      next: (isLoading) =>
        isLoading
          ? this.spinner.show('resources')
          : this.spinner.hide('resources'),
    });
  }
}
