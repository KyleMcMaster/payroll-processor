import { Component, Input, OnInit } from '@angular/core';

import { faSkull, faSmileBeam } from '@fortawesome/free-solid-svg-icons';

import { PayrollListQuery } from './state/payroll-list.query';
import { PayrollListService } from './state/payroll-list.service';

@Component({
  selector: 'app-payroll-list',
  templateUrl: './payroll-list.component.html',
  styleUrls: ['./payroll-list.component.scss'],
})
export class PayrollListComponent implements OnInit {
  readonly faSkull = faSkull;
  readonly faSmileBeam = faSmileBeam;
  readonly payrolls = this.query.selectAll();
  private employeeDepartment: string;

  @Input()
  set department(department: string) {
    const employeeDepartment = department && department.trim();
    this.service.getPayrolls(employeeDepartment);
    this.employeeDepartment = employeeDepartment;
  }

  get department(): string {
    return this.employeeDepartment;
  }

  constructor(
    private query: PayrollListQuery,
    private service: PayrollListService,
  ) {}

  ngOnInit() {
    this.service.getPayrolls(this.department);
  }
}
