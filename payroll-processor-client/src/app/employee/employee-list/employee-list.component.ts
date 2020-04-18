import { Component, OnInit, OnDestroy, Inject } from '@angular/core';
import { Employee } from '../../data/state/models';
import { throwError, of } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { catchError, tap, map } from 'rxjs/operators';
import { DataService } from 'src/app/data/data-service';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { faSmileBeam, faSkull } from '@fortawesome/free-solid-svg-icons';
import { R3FactoryDefMetadataFacade } from '@angular/compiler/src/compiler_facade_interface';

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
