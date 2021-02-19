import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot, CanActivate, CanLoad, Route,
  Router, RouterStateSnapshot, UrlSegment, UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { MsalService } from '@azure/msal-angular';
import { UserSessionQuery } from '@shared/user-session/state/user-session.query';
import { UserSessionService } from '@shared/user-session/state/user-session.service';

@Injectable({
  providedIn: 'root',
})
export class RequireAuthenticatedGuard implements CanActivate, CanLoad {
  constructor(
    private authService: MsalService,
    private router: Router,
    private query: UserSessionQuery,
    private userSessionService: UserSessionService,
  ) {}
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

    // this.userSessionService.getUserSession(msalAccount.accountIdentifier);

    // const canActivate = this.query.select().pipe(
    const canActivate = this.userSessionService
      .getUserSession(msalAccount.accountIdentifier)
      .pipe(
        map((us) => {
          if (isMsalAuthenticated) {
            const isAppAuthenticated =
              us.id !== '00000000-0000-0000-0000-000000000000';

            console.log(
              `User ${msalAccount.accountIdentifier} is Msal authenticated: ${isMsalAuthenticated}`,
            );
            console.log(
              `User ${us.id} is App authenticated: ${!!isAppAuthenticated}`,
            );

            if (isAppAuthenticated) {
              return true;
            }

            this.router.navigate(['/access/registration']);
            return false;
          }

          this.router.navigate(['/access/login']);
          return false;
        }),
      );

    return canActivate;
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

    const canLoad = this.userSessionService
      .getUserSession(msalAccount.accountIdentifier)
      .pipe(
        map((us) => {
          if (isMsalAuthenticated) {
            const isAppAuthenticated =
              us.id !== '00000000-0000-0000-0000-000000000000';

            console.log(
              `User ${msalAccount.accountIdentifier} is Msal authenticated: ${isMsalAuthenticated}`,
            );
            console.log(
              `User ${us.id} is App authenticated: ${!!isAppAuthenticated}`,
            );

            if (isAppAuthenticated) {
              return true;
            }

            this.router.navigate(['/access/registration']);
            return false;
          }

          this.router.navigate(['/access/login']);
          return false;
        }),
      );

    return canLoad;
  }
}
