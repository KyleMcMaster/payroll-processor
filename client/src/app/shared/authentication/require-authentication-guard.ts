import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot, CanActivate, CanLoad, Route,
  Router, RouterStateSnapshot, UrlSegment, UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class RequireAuthenticatedGuard implements CanActivate, CanLoad {
  constructor(private router: Router) {}
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
  ):
    | boolean
    | UrlTree
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree> {
    return false;
  }
  canLoad(
    route: Route,
    segments: UrlSegment[],
  ):
    | boolean
    | UrlTree
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree> {
    return false;
  }
}
