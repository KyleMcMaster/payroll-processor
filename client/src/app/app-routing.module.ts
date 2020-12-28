import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { RequireAuthenticatedGuard } from '@shared/authentication/require-authentication-guard';

const routes: Routes = [
  {
    path: 'employees',
    canLoad: [RequireAuthenticatedGuard],
    loadChildren: () =>
      import('./employee/employee.module').then((m) => m.EmployeeModule),
  },
  {
    path: 'payrolls',
    canLoad: [RequireAuthenticatedGuard],
    loadChildren: () =>
      import('./payroll/payroll.module').then((m) => m.PayrollModule),
  },
  {
    path: 'admin',
    canLoad: [RequireAuthenticatedGuard],
    loadChildren: () =>
      import('./admin/admin.module').then((m) => m.AdminModule),
  },
  {
    path: 'access',
    loadChildren: () =>
      import('./access/access.module').then((m) => m.AccessModule),
  },
  { path: '', pathMatch: 'full', redirectTo: 'employees' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
