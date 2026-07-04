import { Routes } from '@angular/router';
import { AuthLayout } from './layouts/auth-layout/auth-layout';
import { MainLayout } from './layouts/main-layout/main-layout';
import { Login } from './features/auth/login/login';
import { UserDashboard } from './features/users/user-dashboard/user-dashboard';
import { Register } from './features/auth/register/register';
import { ServerError } from './shared/components/server-error/server-error';
import { authGuard } from './core/guards/auth-guard';
import { AccountSettings } from './features/settings/account-settings/account-settings';

export const routes: Routes = [
  {
    path: '',
    component: MainLayout,
    canActivate: [authGuard],
    children: [
      { path: '', component: UserDashboard },
      { path: 'account-settings', component: AccountSettings },
    ],
  },
  {
    path: '',
    component: AuthLayout,
    children: [
      { path: 'login', component: Login },
      { path: 'register', component: Register },
    ],
  },
  {
    path: 'server-error',
    component: ServerError,
  },
];
