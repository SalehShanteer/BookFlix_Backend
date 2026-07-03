import { Injectable } from '@angular/core';
import { ApiService } from './api-service';
import { ISignup } from '../models/auth/signup.model';
import { IToken } from '../models/auth/token.model';
import { Observable, of, tap } from 'rxjs';
import { TokenHelper } from '../../shared/helpers/token-helper';
import { ILogin } from '../models/auth/login.model';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(
    private api: ApiService,
    private router: Router,
  ) {}

  isAuthenticated(): Observable<Boolean> {
    var accessToken = TokenHelper.getAccessToken();
    var refreshToken = TokenHelper.getRefreshToken();
    if (!accessToken || !refreshToken) {
      return of(false);
    }
    return this.api.post<Boolean>('/auth/is-authenticated', { token: refreshToken });
  }

  login(loginModel: ILogin): Observable<IToken> {
    return this.api.post<IToken>('/auth/login', loginModel).pipe(
      tap((res) => {
        TokenHelper.setAccessToken(res.accessToken);
        TokenHelper.setRefreshToken(res.refreshToken);
      }),
    );
  }

  signup(signupModel: ISignup): Observable<IToken> {
    return this.api.post<IToken>('/auth/signup', signupModel).pipe(
      tap((res) => {
        TokenHelper.setAccessToken(res.accessToken);
        TokenHelper.setRefreshToken(res.refreshToken);
      }),
    );
  }

  refreshSession() {
    return this.api.post('/auth/refresh', {});
  }

  logoutBackend() {
    return this.api.post('/auth/logout', {});
  }

  forceLogout() {
    this.router.navigate(['login']);
  }
}
