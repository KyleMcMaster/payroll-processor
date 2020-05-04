import { Component } from '@angular/core';
import { faSkull, faSmileBeam } from '@fortawesome/free-solid-svg-icons';
import { DataService } from 'src/app/data/data-service';
import { Payroll } from 'src/app/data/state/payroll-model';

@Component({
  selector: 'app-payroll-list',
  templateUrl: './payroll-list.component.html',
  styleUrls: ['./payroll-list.component.scss'],
})
export class PayrollListComponent {
  readonly faSkull = faSkull;
  readonly faSmileBeam = faSmileBeam;

  constructor(private dataService: DataService) {}

  getPayroll(): Payroll[] {
    return this.dataService.getPayroll();
  }
}
