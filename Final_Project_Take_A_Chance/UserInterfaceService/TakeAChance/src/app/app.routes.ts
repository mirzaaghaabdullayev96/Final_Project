import { Routes } from '@angular/router';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { RegisterPageComponent } from './pages/register-page/register-page.component';
import { MainPageComponent } from './pages/main-page/main-page.component';
import { LayoutComponent } from './common-ui/layout/layout.component';
import { canActivateAuth, loggedIn } from './auth/access.guard';

export const routes: Routes = [
  {
    path: '', component: LayoutComponent, children: [{ path: '', component: MainPageComponent }],
    canActivate: [canActivateAuth]
  },
  { path: 'login', component: LoginPageComponent, canActivate:[loggedIn] },
  { path: 'register', component: RegisterPageComponent },
];
