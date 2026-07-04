import { Injectable } from '@angular/core';
import { ApiService } from './api-service';
import { ISignup } from '../models/auth/signup.model';
import { Observable, of, tap } from 'rxjs';
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

  private isAuthChecked: boolean = false;
  private isAuthenticated: boolean = false;

  checkAuthStatus(): Observable<boolean> {
    if (this.isAuthChecked) return of(this.isAuthenticated);
    return this.api.post<boolean>('/auth/is-authenticated', {}).pipe(
      tap((isAuthenticatedFromServer) => {
        this.isAuthChecked = true;
        this.isAuthenticated = isAuthenticatedFromServer;
      }),
    );
  }

  login(loginModel: ILogin) {
    return this.api.post('/auth/login', loginModel).pipe(
      tap(() => {
        this.isAuthChecked = true;
        this.isAuthenticated = true;
      }),
    );
  }

  signup(signupModel: ISignup) {
    return this.api.post('/auth/signup', signupModel).pipe(
      tap(() => {
        this.isAuthChecked = true;
        this.isAuthenticated = true;
      }),
    );
  }

  refreshSession() {
    return this.api.post('/auth/refresh', {});
  }

  logoutBackend() {
    return this.api.post('/auth/logout', {}).pipe(
      tap(() => {
        this.isAuthChecked = true;
        this.isAuthenticated = false;
      }),
    );
  }

  forceLogout() {
    this.isAuthChecked = true;
    this.isAuthenticated = false;
    this.router.navigate(['login']);
  }
}
