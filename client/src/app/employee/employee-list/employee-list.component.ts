import { Component, EventEmitter, Input, Output } from '@angular/core';
import { faSkull, faSmileBeam } from '@fortawesome/free-solid-svg-icons';
import { EmployeeListItem } from './state/employee-list.model';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.scss'],
})
export class EmployeeListComponent {
  readonly faSkull = faSkull;
  readonly faSmileBeam = faSmileBeam;

  @Input()
  employees: EmployeeListItem[];

  @Output()
  selected = new EventEmitter<EmployeeListItem>();

  constructor() {}

  setSelected(employee: EmployeeListItem) {
    this.selected.emit(employee);
  }
}
