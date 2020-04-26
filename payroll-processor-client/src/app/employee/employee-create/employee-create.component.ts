import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

import { EmployeeCreate } from '../employee-list/state/employee-list.model';

import { EmployeeCreateService } from './employee-create.service';

@Component({
  selector: 'app-employee-create',
  templateUrl: './employee-create.component.html',
  styleUrls: ['./employee-create.component.scss'],
})
export class EmployeeCreateComponent implements OnInit {
  filterForm = new FormGroup({
    department: new FormControl(''),
    email: new FormControl(''),
    employmentStartedOn: new FormControl(''),
    firstName: new FormControl(''),
    lastName: new FormControl(''),
    phone: new FormControl(''),
    title: new FormControl(''),
  });
  constructor(private employeeCreateService: EmployeeCreateService) {}

  ngOnInit(): void {}

  create() {
    const employee: EmployeeCreate = {
      department: this.filterForm.get('department').value,
      email: this.filterForm.get('email').value,
      employmentStartedOn: this.filterForm
        .get('employmentStartedOn')
        .value.toLocaleDateString(),
      firstName: this.filterForm.get('firstName').value,
      lastName: this.filterForm.get('lastName').value,
      phone: this.filterForm.get('phone').value,
      status: 'ACTIVE',
      title: this.filterForm.get('title').value,
    };

    this.employeeCreateService.createEmployee(employee);
  }
}
