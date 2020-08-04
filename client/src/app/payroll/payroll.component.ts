import { Component } from '@angular/core';

import { faLock, faUnlock } from '@fortawesome/free-solid-svg-icons';

import { PayrollListQuery } from './payroll-list/state/payroll-list.query';
import { PayrollListService } from './payroll-list/state/payroll-list.service';

@Component({
  selector: 'app-payroll',
  templateUrl: './payroll.component.html',
  styleUrls: ['./payroll.component.scss'],
})
export class PayrollComponent {
  readonly faLock = faLock;
  readonly faUnlock = faUnlock;
  readonly payrolls = this.query.selectAll();
  department = 'Building_Services';

  readonly departments: Array<string> = [
    'Building_Services',
    'Human_Resources',
    'IT',
    'Marketing',
    'Sales',
    'Warehouse',
  ];

  constructor(
    private query: PayrollListQuery,
    private service: PayrollListService,
  ) {
    this.service.getPayrolls(this.department);
  }

  onSelectDepartment(department: string) {
    this.service.getPayrolls(department);
    this.department = department;
  }
}
