import { ChangeDetectionStrategy, Component } from '@angular/core';
import { UntypedFormControl, UntypedFormGroup, Validators } from '@angular/forms';

import { faSyncAlt } from '@fortawesome/free-solid-svg-icons';

import { ResourcesQuery } from './state/resources.query';
import { ResourcesService } from './state/resources.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminComponent {
  readonly faSyncAlt = faSyncAlt;
  readonly isLoading = this.query.selectLoading();
  readonly stats = this.query.stats;

  readonly form = new UntypedFormGroup({
    totalEmployees: new UntypedFormControl(1, { validators: [Validators.min(1)] }),
    maxPayrolls: new UntypedFormControl(1, { validators: [Validators.min(1)] }),
  });

  constructor(
    private readonly query: ResourcesQuery,
    private readonly service: ResourcesService,
  ) {
    this.service.getStats();
  }

  onRefreshStats() {
    this.service.getStats();
  }

  onCreate() {
    this.service.create(
      this.form.value.totalEmployees,
      this.form.value.maxPayrolls,
    );
  }

  onReset() {
    this.service.reset();
  }
}
