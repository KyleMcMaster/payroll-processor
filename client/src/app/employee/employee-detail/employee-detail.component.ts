import { Component, Input } from '@angular/core';

import { EmployeeDetail } from './state/employee.model';

@Component({
  selector: 'app-employee-detail',
  templateUrl: './employee-detail.component.html',
  styleUrls: ['./employee-detail.component.scss'],
})
export class EmployeeDetailComponent {
  @Input()
  employee: EmployeeDetail;

  constructor() {}
}
