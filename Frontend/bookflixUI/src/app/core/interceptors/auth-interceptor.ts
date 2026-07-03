import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, switchMap, throwError, EMPTY } from 'rxjs';
import { AuthService } from '../services/auth-service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const secureReq = req.clone({
    withCredentials: true,
  });

  return next(secureReq).pipe(
    catchError((error) => {
      if (error instanceof HttpErrorResponse) {
        if (error.status === 401) {
          return authService.refreshSession().pipe(
            switchMap(() => {
              return next(secureReq);
            }),
            catchError((refreshError) => {
              authService.forceLogout();
              return throwError(() => refreshError);
            }),
          );
        }
        if (error.status >= 500) {
          router.navigate(['/server-error']);
          return EMPTY;
        }
      }
      return throwError(() => error);
    }),
  );
};
