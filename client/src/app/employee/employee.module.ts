import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';

import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { SharedModule } from '@shared/shared.module';

import { EmployeeCreateComponent } from './employee-create/employee-create.component';
import { EmployeeDetailComponent } from './employee-detail/employee-detail.component';
import { EmployeeListComponent } from './employee-list/employee-list.component';
import { EmployeePayrollCreateComponent } from './employee-payroll-create/employee-payroll-create.component';
import { EmployeePayrollListComponent } from './employee-payroll-list/employee-payroll-list.component';
import { EmployeeComponent } from './employee.component';

const routes: Routes = [{ path: '', component: EmployeeComponent }];

@NgModule({
  declarations: [
    EmployeeComponent,
    EmployeeCreateComponent,
    EmployeeDetailComponent,
    EmployeeListComponent,
    EmployeePayrollCreateComponent,
    EmployeePayrollListComponent,
  ],
  imports: [
    RouterModule.forChild(routes),
    NgbModule,
    FontAwesomeModule,
    CommonModule,
    ReactiveFormsModule,
    SharedModule,
  ],
})
export class EmployeeModule {}
