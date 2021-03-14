import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  CanLoad,
  Route,
  Router,
  RouterStateSnapshot,
  UrlSegment,
  UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';

import { AuthService } from '@auth0/auth0-angular';
import { UserSessionQuery } from '@shared/user-session/state/user-session.query';
import { UserSessionService } from '@shared/user-session/state/user-session.service';

@Injectable({
  providedIn: 'root',
})
export class RequireAuthenticatedGuard implements CanActivate, CanLoad {
  constructor(
    private authService: AuthService,
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

    return this.authService.isAuthenticated$;
  }

  canLoad(
    route: Route,
    segments: UrlSegment[],
  ):
    | boolean
    | UrlTree
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree> {
      return this.authService.isAuthenticated$;
  }
}
