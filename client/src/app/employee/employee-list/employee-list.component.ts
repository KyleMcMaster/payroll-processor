import {
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import {
  faPlus,
  faSkull,
  faSmileBeam,
} from '@fortawesome/free-solid-svg-icons';
import { Observable } from 'rxjs';
import { EmployeeList } from './state/employee-list.model';

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
  employees: Observable<EmployeeList[]>;

  @Output()
  selected = new EventEmitter<EmployeeList>();

  constructor() {}

  ngOnInit() {}

  ngOnDestroy() {}

  setSelected(employee: EmployeeList) {
    this.selected.emit(employee);
  }
}
