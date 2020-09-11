import { Component, Input } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

import { EmployeeDetail, EmployeePayrollCreate } from '@employee/employee-detail/state/employee-detail.model';
import { EmployeeDetailService } from '@employee/employee-detail/state/employee-detail.service';
import { ClockService } from '@shared/clock.service';

@Component({
  selector: 'app-employee-payroll-create',
  templateUrl: './employee-payroll-create.component.html',
  styleUrls: ['./employee-payroll-create.component.scss'],
})
export class EmployeePayrollCreateComponent {
  filterForm = new FormGroup({
    checkDate: new FormControl(this.clock.now().getDate()),
    employeeId: new FormControl(''),
    grossPayroll: new FormControl(0),
  });

  @Input()
  set employee(employee: EmployeeDetail) {
    this.filterForm.patchValue({
      employeeId: employee.id,
      checkDate: this.clock.now(),
      grossPayroll: 0,
    });
  }

  constructor(
    private detailService: EmployeeDetailService,
    private clock: ClockService,
  ) {}

  create(): void {
    const payroll: EmployeePayrollCreate = {
      checkDate: this.filterForm.get('checkDate').value,
      employeeId: this.filterForm.get('employeeId').value,
      grossPayroll: this.filterForm.get('grossPayroll').value,
    };

    this.detailService.createPayroll(payroll);
  }
}
