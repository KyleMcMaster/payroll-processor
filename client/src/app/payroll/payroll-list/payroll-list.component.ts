import { Component, Input } from '@angular/core';

import { PayrollListItem } from '@payroll/payroll-list/state/payroll-list.model';

@Component({
  selector: 'app-payroll-list',
  templateUrl: './payroll-list.component.html',
  styleUrls: ['./payroll-list.component.scss'],
})
export class PayrollListComponent {
  @Input()
  payrolls: PayrollListItem[];

  constructor() {}
}
