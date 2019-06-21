import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';

import { HttpClientModule } from '@angular/common/http';
import {MatGridListModule} from '@angular/material/grid-list';
import {MatCardModule} from '@angular/material/card';
import { NoValuePipe } from './no-value.pipe';

@NgModule({
  imports: [ 
              BrowserModule, 
              FormsModule, 
              HttpClientModule, 
              MatGridListModule, 
              MatCardModule 
            ],
  declarations: [ AppComponent, NoValuePipe ],
  bootstrap:    [ AppComponent ]
})
export class AppModule { }
