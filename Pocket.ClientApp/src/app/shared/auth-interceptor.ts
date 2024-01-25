import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, HttpErrorResponse } from "@angular/common/http";
import { Router } from "@angular/router";
import { catchError, Observable, throwError } from "rxjs";
import { UserStore } from "../core/state/user.store";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private router: Router, private userStore: UserStore) { }

  private handleAuthError(err: HttpErrorResponse): Observable<any> {
    let errorMsg;
    if (err.error instanceof Error) {
      // A client-side or network error occurred. Handle it accordingly.
      errorMsg = `An error occurred: ${err.error.message}`;
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      errorMsg = `Backend returned code ${err.status}, body was: ${err.error}`;
    }
    if (err.status === 401 || err.status === 403) {
      this.userStore.setToken(undefined);
      this.router.navigateByUrl(`/user/login`);
    }
    console.error(errorMsg);

    return throwError(() => err);
  }

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    const authToken = this.userStore?.current.token;
    if (authToken) {
      req = req.clone({
        setHeaders: {
          Authorization: "Bearer " + authToken
        }
      });
    }
    return next.handle(req).pipe(catchError(x => this.handleAuthError(x)));;
  }
}
