import { Routes } from '@angular/router';
import { LoginPageComponent } from './pages/login-register/login-page/login-page.component';
import { RegisterPageComponent } from './pages/login-register/register-page/register-page.component';
import { MainPageComponent } from './pages/main-page/main-page.component';
import { LayoutComponent } from './common-ui/layout/layout.component';
import { canActivateAuth, loggedIn } from './auth/access.guard';
import { ForgotPasswordPageComponent } from './pages/login-register/forgot-password-page/forgot-password-page.component';
import { VerifyEmailPageComponent } from './pages/login-register/verify-email-page/verify-email-page.component';
import { EmailVerifiedPageComponent } from './pages/login-register/email-verified-page/email-verified-page.component';
import { ResetPasswordPageComponent } from './pages/login-register/reset-password-page/reset-password-page.component';
import { PasswordResetedPageComponent } from './pages/login-register/password-reseted-page/password-reseted-page.component';
import { ForgotPasswordEmailPageComponent } from './pages/login-register/forgot-password-email-page/forgot-password-email-page.component';
import { resetPasswordGuard } from './auth/reset-password.guard';
import { AdminMainPageComponent } from './pages/admin-panel/admin-main-page/admin-main-page.component';

export const routes: Routes = [
  {
    path: '', component: LayoutComponent, children: [{ path: '', component: MainPageComponent }],
    canActivate: [canActivateAuth]
  },
  { path: 'login', component: LoginPageComponent, canActivate:[loggedIn] },
  { path: 'register', component: RegisterPageComponent },
  { path: 'forgot-password', component: ForgotPasswordPageComponent },
  { path: 'verify-email', component: VerifyEmailPageComponent },
  { path: 'email-verified', component: EmailVerifiedPageComponent },
  { path: 'reset-password', component: ResetPasswordPageComponent, canActivate: [resetPasswordGuard] } ,
  { path: 'password-reseted', component: PasswordResetedPageComponent },
  { path: 'forgot-password-link', component: ForgotPasswordEmailPageComponent },
  { path: 'admin', component: AdminMainPageComponent },
];
