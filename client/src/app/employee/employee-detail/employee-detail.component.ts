import { DatePipe } from '@angular/common';
import { Component, Input } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import {
  EmployeeDetail,
  EmployeeUpdate,
} from '@employee/employee-detail/state/employee-detail.model';
import { EmployeeDetailService } from '@employee/employee-detail/state/employee-detail.service';

@Component({
  selector: 'app-employee-detail',
  templateUrl: './employee-detail.component.html',
  styleUrls: ['./employee-detail.component.scss'],
  providers: [DatePipe],
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
    id: new FormControl(''),
    department: new FormControl(''),
    email: new FormControl(''),
    employmentStartedOn: new FormControl(''),
    firstName: new FormControl(''),
    lastName: new FormControl(''),
    phone: new FormControl(''),
    status: new FormControl(''),
    title: new FormControl(''),
    version: new FormControl(''),
  });

  @Input()
  set employee(employee: EmployeeDetail) {
    this.filterForm.patchValue({
      ...employee,
      employmentStartedOn: this.datePipe.transform(
        employee.employmentStartedOn,
        'yyyy-MM-dd',
      ),
    });
  }

  constructor(
    private datePipe: DatePipe,
    private service: EmployeeDetailService,
  ) {}

  update() {
    const employee: EmployeeUpdate = {
      id: this.filterForm.get('id').value,
      department: this.filterForm.get('department').value,
      employmentStartedOn: this.filterForm.get('employmentStartedOn').value,
      email: this.filterForm.get('email').value,
      firstName: this.filterForm.get('firstName').value,
      lastName: this.filterForm.get('lastName').value,
      phone: this.filterForm.get('phone').value,
      status: this.filterForm.get('status').value,
      title: this.filterForm.get('title').value,
      version: this.filterForm.get('').value,
    };

    this.service.update(employee);
  }
}
