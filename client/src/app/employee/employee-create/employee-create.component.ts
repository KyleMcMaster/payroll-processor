import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

import { EmployeeCreate } from '@employee/employee-list/state/employee-list.model';
import { EmployeeListService } from '@employee/employee-list/state/employee-list.service';

@Component({
  selector: 'app-employee-create',
  templateUrl: './employee-create.component.html',
  styleUrls: ['./employee-create.component.scss'],
})
export class EmployeeCreateComponent {
  filterForm = new FormGroup({
    department: new FormControl(''),
    email: new FormControl(''),
    employmentStartedOn: new FormControl(''),
    firstName: new FormControl(''),
    lastName: new FormControl(''),
    phone: new FormControl(''),
    title: new FormControl(''),
  });

  readonly departments: string[] = [
    'Building_Services',
    'Human_Resources',
    'IT',
    'Marketing',
    'Sales',
    'Warehouse',
  ];

  constructor(private employeeListService: EmployeeListService) {}

  create() {
    const employee: EmployeeCreate = {
      department: this.filterForm.get('department').value,
      email: this.filterForm.get('email').value,
      employmentStartedOn: this.filterForm.get('employmentStartedOn').value,
      firstName: this.filterForm.get('firstName').value,
      lastName: this.filterForm.get('lastName').value,
      phone: this.filterForm.get('phone').value,
      status: 'Enabled',
      title: this.filterForm.get('title').value,
    };

    this.employeeListService.createEmployee(employee);
  }
}
