import { Component, EventEmitter, Input, Output } from '@angular/core';
import { faSkull, faSmileBeam } from '@fortawesome/free-solid-svg-icons';
import { EmployeeListItem } from './state/employee-list.model';

@Component({
  selector: 'app-employee-list',
  template: `
    <div class="row" *ngFor="let employee of employees">
      <div
        class="list-group-item list-group-item-action"
        (click)="selected.emit(employee)"
      >
        <div class="row">
          <div class="col-10">
            <span> {{ employee.firstName }} {{ employee.lastName }} </span>
          </div>
          <div class="col-2">
            <fa-icon *ngIf="employee.status === 'Enabled'" [icon]="faSmileBeam">
            </fa-icon>
            <fa-icon *ngIf="employee.status === 'Disabled'" [icon]="faSkull">
            </fa-icon>
          </div>
        </div>
      </div>
    </div>
  `,
})
export class EmployeeListComponent {
  readonly faSkull = faSkull;
  readonly faSmileBeam = faSmileBeam;

  @Input()
  employees: EmployeeListItem[];

  @Output()
  selected = new EventEmitter<EmployeeListItem>();

  constructor() {}
}
