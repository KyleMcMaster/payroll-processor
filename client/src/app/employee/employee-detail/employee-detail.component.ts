import { Component, Input } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

import { EmployeeDetail } from '@employee/employee-detail/state/employee-detail.model';
import { EmployeeDetailService } from '@employee/employee-detail/state/employee-detail.service';

@Component({
  selector: 'app-employee-detail',
  templateUrl: './employee-detail.component.html',
  styleUrls: ['./employee-detail.component.scss'],
})
export class EmployeeDetailComponent {
  readonly departments: string[] = [
    'Building_Services',
    'Human_Resources',
    'IT',
    'Marketing',
    'Sales',
    'Warehouse',
  ];

  filterForm = new FormGroup({
    department: new FormControl(''),
    email: new FormControl(''),
    employmentStartedOn: new FormControl(''),
    firstName: new FormControl(''),
    lastName: new FormControl(''),
    phone: new FormControl(''),
    title: new FormControl(''),
  });

  @Input()
  employee: EmployeeDetail;

  constructor(private employeeDetailService: EmployeeDetailService) {}

  update() {
    this.employeeDetailService.update(this.employee);
  }
}
