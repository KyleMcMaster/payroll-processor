import { Component, OnDestroy, OnInit } from '@angular/core';
import { faSkull, faSmileBeam } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Employee } from './state/employee-list.model';
import { EmployeeListQuery } from './state/employee-list.query';
import { EmployeeListService } from './state/employee-list.service';
import { EmployeeListStore } from './state/employee-list.store';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.scss'],
})
export class EmployeeListComponent implements OnInit, OnDestroy {
  readonly faSkull = faSkull;
  readonly faSmileBeam = faSmileBeam;
  readonly employees: Observable<Employee[]>;

  constructor(
    private service: EmployeeListService,
    private query: EmployeeListQuery,
    private store: EmployeeListStore,
    private toastr: ToastrService,
  ) {
    this.employees = this.query.selectAll();
    this.service.getEmployees();
  }

  ngOnInit() {}

  ngOnDestroy() {}

  updateEmployeeStatus(employee: Employee) {
    this.service
      .updateEmployeeStatus(employee)
      .pipe(
        catchError((err) => {
          console.log(`Could not update employee ${employee.id}`);
          return throwError(err);
        }),
      )
      .subscribe({
        next: (detail) => {
          this.toastr.show('Employee sucessfully updated!');
          this.store.upsert(detail.id, detail);
        },
        complete: () => this.store.setLoading(false),
      });
  }
}
