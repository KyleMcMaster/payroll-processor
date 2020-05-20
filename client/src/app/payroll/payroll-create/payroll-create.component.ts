import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';
import { Employee } from 'src/app/employee/employee-list/state/employee-list.model';
import { EmployeeListQuery } from 'src/app/employee/employee-list/state/employee-list.query';
import { EmployeeListService } from 'src/app/employee/employee-list/state/employee-list.service';
import { PayrollCreate } from '../payroll-list/state/payroll-list.model';
import { PayrollCreateService } from './payroll-create.service';

@Component({
  selector: 'app-payroll-create',
  templateUrl: './payroll-create.component.html',
  styleUrls: ['./payroll-create.component.scss'],
})
export class PayrollCreateComponent implements OnInit {
  filterForm = new FormGroup({
    employeeId: new FormControl(''),
    grossPayroll: new FormControl(''),
    PayrollPeriod: new FormControl(''),
    checkDate: new FormControl(''),
  });
  readonly employees: Observable<Employee[]>;

  constructor(
    private employeeService: EmployeeListService,
    private employeeQuery: EmployeeListQuery,
    private payrollService: PayrollCreateService,
  ) {
    this.employees = this.employeeQuery.selectAll();
    this.employeeService.getEmployees();
  }

  ngOnInit(): void {}

  create() {
    const payroll: PayrollCreate = {
      checkDate: this.filterForm.get('checkDate').value,
      employeeId: this.filterForm.get('employeeId').value,
      grossPayroll: this.filterForm.get('grossPayroll').value,
      payrollPeriod: this.filterForm.get('payrollPeriod').value,
    };

    this.payrollService.createPayroll(payroll);
  }
}
