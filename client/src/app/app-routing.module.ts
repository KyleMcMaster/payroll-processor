import { NgModule } from '@angular/core';
import {
  RouterModule,
  Routes,
} from '@angular/router';

import { RequireAuthenticatedGuard } from '@shared/authentication/require-authentication-guard';
import { RequireUnauthenticatedGuard } from '@shared/authentication/require-unauthenticated-guard';

import { LoginComponent } from './access/login/login.component';
import { LogoutComponent } from './access/logout/logout.component';
import { RegistrationComponent } from './access/registration/registration.component';

const routes: Routes = [
  {
    path: 'employees',
    canActivate: [RequireAuthenticatedGuard],
    canLoad: [RequireAuthenticatedGuard],
    loadChildren: () =>
      import('./employee/employee.module').then((m) => m.EmployeeModule),
  },
  {
    path: 'payrolls',
    canActivate: [RequireAuthenticatedGuard],
    canLoad: [RequireAuthenticatedGuard],
    loadChildren: () =>
      import('./payroll/payroll.module').then((m) => m.PayrollModule),
  },
  {
    path: 'admin',
    canActivate: [RequireAuthenticatedGuard],
    canLoad: [RequireAuthenticatedGuard],
    loadChildren: () =>
      import('./admin/admin.module').then((m) => m.AdminModule),
  },
  // {
  //   path: 'access',
  //   loadChildren: () =>
  //     import('./access/access.module').then((m) => m.AccessModule),
  // },
  {
    path: 'login',
    canActivate: [RequireUnauthenticatedGuard],
    canLoad: [RequireUnauthenticatedGuard],
    component: LoginComponent,
  },
  { path: 'logout', component: LogoutComponent },
  {
    path: 'registration',
    component: RegistrationComponent,
  },
  { path: '', pathMatch: 'full', redirectTo: 'employees' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
