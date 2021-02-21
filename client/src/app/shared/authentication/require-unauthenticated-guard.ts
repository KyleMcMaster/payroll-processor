import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  CanLoad,
  Route,
  RouterStateSnapshot,
  UrlSegment,
  UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';

import { MsalService } from '@azure/msal-angular';

@Injectable({
  providedIn: 'root',
})
export class RequireUnauthenticatedGuard implements CanActivate, CanLoad {
  constructor(private authService: MsalService) {}
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
  ):
    | boolean
    | UrlTree
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree> {
    const msalAccount = this.authService.getAccount();
    const isMsalAuthenticated = !!msalAccount;

    return !isMsalAuthenticated;
  }

  canLoad(
    route: Route,
    segments: UrlSegment[],
  ):
    | boolean
    | UrlTree
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree> {
    const msalAccount = this.authService.getAccount();
    const isMsalAuthenticated = !!msalAccount;

    return !isMsalAuthenticated;
  }
}
