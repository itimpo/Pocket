import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
  UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';
import { UserStore } from '../core/state/user.store';
@Injectable({
  providedIn: 'root',
})
export class AuthGuard {
  constructor(public userStore: UserStore, public router: Router) { }
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | UrlTree | boolean {
    if (!this.userStore.current.token) {
      this.router.navigate(['user/login']);
    }
    return true;
  }
}
