import { Component, Input } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { EmployeeDetail } from '@employee/employee-detail/state/employee-detail.model';
import { EmployeeDetailService } from '@employee/employee-detail/state/employee-detail.service';

@Component({
  selector: 'app-employee-payroll-create',
  templateUrl: './employee-payroll-create.component.html',
  styleUrls: ['./employee-payroll-create.component.scss'],
})
export class EmployeePayrollCreateComponent {
  filterform = new FormGroup({
    payrollPeriod: new FormControl('payrollPeriod'),
  });

  @Input()
  employee: EmployeeDetail;

  constructor(private detailService: EmployeeDetailService) {}

  create(): void {}
}
