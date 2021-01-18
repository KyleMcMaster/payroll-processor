import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ToastrModule } from 'ngx-toastr';

import { MsalModule } from '@azure/msal-angular';
import { SharedModule } from '@shared/shared.module';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';

const NG_MODULES = [BrowserAnimationsModule, BrowserModule, HttpClientModule];

const THIRD_PARTY_MODULES = [
  FontAwesomeModule,
  NgbModule,
  ToastrModule.forRoot({
    timeOut: 10000,
    positionClass: 'toast-bottom-right',
    preventDuplicates: false,
  }),
  MsalModule.forRoot(
    {
      auth: {
        clientId: 'CLIENT_ID',
        authority:
          'AUTHORITY_URL/TENANT_ID',
        redirectUri: 'https://localhost:4201/',
      },
      cache: {
        cacheLocation: 'localStorage',
        storeAuthStateInCookie: false,
      },
    },
    {
      popUp: true,
      consentScopes: ['user.read', 'openid', 'profile'],
      protectedResourceMap: [
        ['Enter_the_Graph_Endpoint_Herev1.0/me', ['user.read']],
      ],
      extraQueryParameters: {},
    },
  ),
];

const APP_MODULES = [AppRoutingModule, CoreModule, SharedModule];

@NgModule({
  declarations: [AppComponent],
  imports: [NG_MODULES, THIRD_PARTY_MODULES, APP_MODULES],
  bootstrap: [AppComponent],
})
export class AppModule {}
