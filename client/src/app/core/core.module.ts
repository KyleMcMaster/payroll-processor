import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { NotFoundComponent } from './not-found.component';

const routes = [{ path: '**', component: NotFoundComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  declarations: [NotFoundComponent],
})
export class CoreModule {}
