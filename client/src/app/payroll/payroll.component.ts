import { Component, OnInit } from '@angular/core';
import { faLock, faUnlock } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-payroll',
  templateUrl: './payroll.component.html',
  styleUrls: ['./payroll.component.scss'],
})
export class PayrollComponent implements OnInit {
  readonly faLock = faLock;
  readonly faUnlock = faUnlock;
  department = 'IT';
  isReadonly = true;

  constructor() {}

  ngOnInit() {}

  toggleReadonly() {
    this.isReadonly = !this.isReadonly;
  }
}
