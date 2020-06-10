import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { faPlus, faSkull, faSmileBeam } from '@fortawesome/free-solid-svg-icons';
import { EmployeeListItem } from './state/employee-list.model';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.scss'],
})
export class EmployeeListComponent implements OnInit, OnDestroy {
  readonly faSkull = faSkull;
  readonly faSmileBeam = faSmileBeam;
  readonly faPlus = faPlus;

  @Input()
  employees: EmployeeListItem[];

  @Output()
  selected = new EventEmitter<EmployeeListItem>();

  constructor() { }

  ngOnInit() { }

  ngOnDestroy() { }

  setSelected(employee: EmployeeListItem) {
    this.selected.emit(employee);
  }
}
