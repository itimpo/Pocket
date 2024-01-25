
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserLogin, UserToken } from '../models/user';
import { BaseService } from './_base.service';

@Injectable({
  providedIn: 'root'
})
export class UserService extends BaseService{

  url = {
    login: '/users/login',
  }

  //#region User

  login(post: UserLogin): Observable<UserToken> {
    return super.post<UserLogin, UserToken>(`${this.url.login}`, post);
  }

  //#endregion
}
