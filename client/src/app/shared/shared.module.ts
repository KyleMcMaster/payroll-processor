import { NgModule } from '@angular/core';
import { UnslugifyPipe } from './unslugify.pipe';

@NgModule({
  declarations: [UnslugifyPipe],
  exports: [UnslugifyPipe],
})
export class SharedModule {}
