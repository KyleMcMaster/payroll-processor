import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SharedModule } from '@shared/shared.module';

import { PayrollListComponent } from './payroll-list/payroll-list.component';
import { PayrollComponent } from './payroll.component';

const routes: Routes = [{ path: '', component: PayrollComponent }];

@NgModule({
  declarations: [PayrollComponent, PayrollListComponent],
  imports: [RouterModule.forChild(routes), CommonModule, SharedModule],
})
export class PayrollModule {}
