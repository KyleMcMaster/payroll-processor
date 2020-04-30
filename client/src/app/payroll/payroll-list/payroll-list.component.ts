import { Component } from '@angular/core';
import { Payroll } from 'src/app/data/state/payroll-model';
import { faSmileBeam, faSkull } from '@fortawesome/free-solid-svg-icons';
import { DataService } from 'src/app/data/data-service';

@Component({
  selector: 'app-payroll-list',
  templateUrl: './payroll-list.component.html',
  styleUrls: ['./payroll-list.component.scss'],
})
export class PayrollListComponent {
  faSkull = faSkull;
  faSmileBeam = faSmileBeam;

  constructor(private dataService: DataService) {}

  getPayroll(): Payroll[] {
    return this.dataService.getPayroll();
  }
}
