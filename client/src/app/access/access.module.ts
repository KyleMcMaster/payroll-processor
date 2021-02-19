import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';

import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

import { RequireUnauthenticatedGuard } from '@shared/authentication/require-unauthenticated-guard';
import { SharedModule } from '@shared/shared.module';

import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { RegistrationComponent } from './registration/registration.component';

const routes: Routes = [
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
  { path: '', pathMatch: 'full', redirectTo: 'login' },
];

@NgModule({
  declarations: [LoginComponent, LogoutComponent, RegistrationComponent],
  imports: [
    RouterModule.forChild(routes),
    CommonModule,
    SharedModule,
    FontAwesomeModule,
    ReactiveFormsModule,
  ],
})
export class AccessModule {}
