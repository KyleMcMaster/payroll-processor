import { Component } from '@angular/core';
import { departments } from '@department/department.model';
import { faLock, faUnlock } from '@fortawesome/free-solid-svg-icons';
import { PayrollListQuery } from '@payroll/payroll-list/state/payroll-list.query';
import { PayrollListService } from '@payroll/payroll-list/state/payroll-list.service';

@Component({
  selector: 'app-payroll',
  templateUrl: './payroll.component.html',
  styleUrls: ['./payroll.component.scss'],
})
export class PayrollComponent {
  readonly faLock = faLock;
  readonly faUnlock = faUnlock;
  readonly payrolls = this.query.selectAll();
  selectedDepartment = 'Building_Services';

  readonly departments = departments;

  constructor(
    private query: PayrollListQuery,
    private service: PayrollListService,
  ) {
    this.service.getPayrolls(this.selectedDepartment);
  }

  onSelectDepartment(department: string) {
    this.service.getPayrolls(department);
    this.selectedDepartment = department;
  }
}
