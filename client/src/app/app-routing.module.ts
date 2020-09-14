import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: 'employees',
    loadChildren: () =>
      import('./employee/employee.module').then((m) => m.EmployeeModule),
  },
  {
    path: 'payrolls',
    loadChildren: () =>
      import('./payroll/payroll.module').then((m) => m.PayrollModule),
  },
  {
    path: 'admin',
    loadChildren: () =>
      import('./admin/admin.module').then((m) => m.AdminModule),
  },
  { path: '', pathMatch: 'full', redirectTo: 'employees' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
