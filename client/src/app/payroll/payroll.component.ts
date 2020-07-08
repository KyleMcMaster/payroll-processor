import { Component, OnInit } from '@angular/core';

import { faLock, faUnlock } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-payroll',
  templateUrl: './payroll.component.html',
  styleUrls: ['./payroll.component.scss'],
})
export class PayrollComponent implements OnInit {
  readonly faLock = faLock;
  readonly faUnlock = faUnlock;
  department = 'Building_Services';

  readonly departments: Array<string> = [
    'Building_Services',
    'Human_Resources',
    'IT',
    'Marketing',
    'Sales',
    'Warehouse',
  ];

  constructor() {}

  ngOnInit() {}

  onSelectDepartment(department: string) {
    this.department = department;
  }
}
