import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Employee } from './state/employee-detail.model';

@Component({
  selector: 'app-employee-detail',
  templateUrl: './employee-detail.component.html',
  styleUrls: ['./employee-detail.component.scss'],
})
export class EmployeeDetailComponent implements OnInit {
  @Input()
  employee: Observable<Employee>;

  constructor() {}

  ngOnInit() {}
}
