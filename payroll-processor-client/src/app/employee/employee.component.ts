import { Component } from '@angular/core';

import { faLock, faUnlock } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.scss'],
})
export class EmployeeComponent {
  readonly faLock = faLock;
  readonly faUnlock = faUnlock;
  isReadonly = true;

  constructor() {}

  toggleReadonly() {
    this.isReadonly = !this.isReadonly;
  }
}
