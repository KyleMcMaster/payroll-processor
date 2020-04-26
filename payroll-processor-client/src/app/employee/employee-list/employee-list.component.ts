import { Component, OnDestroy, OnInit } from '@angular/core';

import { faSkull, faSmileBeam } from '@fortawesome/free-solid-svg-icons';

import { Employee } from '../../data/state/employee-model';

import { EmployeeListService } from './state/employee-list.service';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.scss'],
})
export class EmployeeListComponent implements OnInit, OnDestroy {
  faSkull = faSkull;
  faSmileBeam = faSmileBeam;

  constructor(private employeeListService: EmployeeListService) {}

  ngOnInit() {}

  ngOnDestroy() {}

  getEmployees(): Employee[] {
    return this.employeeListService.getEmployees();
  }

  add() {}

  remove(id: string) {
    // this.dataService.removeEmployee(id);
  }
}
