import { DatePipe } from '@angular/common';
import { Component, Input } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { departments } from '@department/department.model';
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
  readonly departments = departments;

  filterForm = new FormGroup({
    department: new FormControl({ value: '', disabled: true }),
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

    this.filterForm.patchValue({
      ...this._employee,
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
      ...this._employee,
      email: this.filterForm.get('email').value,
      employmentStartedOn: this.filterForm.get('employmentStartedOn').value,
      firstName: this.filterForm.get('firstName').value,
      lastName: this.filterForm.get('lastName').value,
      phone: this.filterForm.get('phone').value,
      status: this.filterForm.get('status').value,
      title: this.filterForm.get('title').value,
    };

    this.service.update(employee);
  }
}
