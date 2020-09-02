import { Component, Input } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

import { EmployeeDetail, EmployeeUpdate } from '@employee/employee-detail/state/employee-detail.model';
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

  readonly filterForm = new FormGroup({
    department: new FormControl(''),
    email: new FormControl(''),
    employmentStartedOn: new FormControl(''),
    firstName: new FormControl(''),
    lastName: new FormControl(''),
    phone: new FormControl(''),
    status: new FormControl(''),
    title: new FormControl(''),
  });

  private _employee: EmployeeDetail;

  @Input()
  get employee(): EmployeeDetail {
    return this._employee;
  }
  set employee(employee: EmployeeDetail) {
    this._employee = employee;
    this.filterForm.patchValue({ ...employee });
  }

  constructor(private employeeDetailService: EmployeeDetailService) {}

  update() {
    const employee: EmployeeUpdate = {
      ...this._employee,
      department: this.filterForm.get('department').value,
      employmentStartedOn: this.filterForm.get('employmentStartedOn').value,
      firstName: this.filterForm.get('firstName').value,
      lastName: this.filterForm.get('lastName').value,
      phone: this.filterForm.get('phone').value,
      status: this.filterForm.get('status').value,
      title: this.filterForm.get('title').value,
    };

    this.employeeDetailService.update(employee);
  }
}
