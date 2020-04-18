import { Component, OnInit, OnDestroy } from '@angular/core';
import { Employee } from '../../data/state/employee-model';
import { DataService } from 'src/app/data/data-service';
import { faSmileBeam, faSkull } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.scss'],
})
export class EmployeeListComponent implements OnInit, OnDestroy {
  faSkull = faSkull;
  faSmileBeam = faSmileBeam;

  constructor(private dataService: DataService) {}

  ngOnInit() {}

  ngOnDestroy() {}

  getEmployees(): Employee[] {
    return this.dataService.getEmployees();
  }

  add() {}

  remove(id: string) {
    this.dataService.removeEmployee(id);
  }
}
