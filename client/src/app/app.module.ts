import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { EmployeeCreateComponent } from '@employee/employee-create/employee-create.component';
import { EmployeeDetailComponent } from '@employee/employee-detail/employee-detail.component';
import { EmployeeListComponent } from '@employee/employee-list/employee-list.component';
import { EmployeePayrollListComponent } from '@employee/employee-payroll-list/employee-payroll-list.component';
import { EmployeeComponent } from '@employee/employee.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { PayrollListComponent } from '@payroll/payroll-list/payroll-list.component';
import { PayrollComponent } from '@payroll/payroll.component';
import { SharedModule } from '@shared/shared.module';
import { ToastrModule } from 'ngx-toastr';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { EmployeePayrollCreateComponent } from './employee/employee-payroll-create/employee-payroll-create.component';

@NgModule({
  declarations: [
    AppComponent,
    EmployeeComponent,
    EmployeeCreateComponent,
    EmployeeDetailComponent,
    EmployeeListComponent,
    EmployeePayrollListComponent,
    PayrollComponent,
    PayrollListComponent,
    EmployeePayrollCreateComponent,
  ],
  imports: [
    AppRoutingModule,
    BrowserAnimationsModule,
    BrowserModule,
    FontAwesomeModule,
    FormsModule,
    HttpClientModule,
    NgbModule,
    ReactiveFormsModule,
    SharedModule,
    ToastrModule.forRoot({
      timeOut: 10000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: false,
    }),
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
