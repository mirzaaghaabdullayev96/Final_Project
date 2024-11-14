import { Routes } from '@angular/router';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { RegisterPageComponent } from './pages/register-page/register-page.component';
import { MainPageComponent } from './pages/main-page/main-page.component';
import { LayoutComponent } from './common-ui/layout/layout.component';
import { canActivateAuth, loggedIn } from './auth/access.guard';
import { ForgotPasswordPageComponent } from './pages/forgot-password-page/forgot-password-page.component';
import { VerifyEmailPageComponent } from './pages/verify-email-page/verify-email-page.component';
import { EmailVerifiedPageComponent } from './pages/email-verified-page/email-verified-page.component';
import { ResetPasswordPageComponent } from './pages/reset-password-page/reset-password-page.component';
import { PasswordResetedPageComponent } from './pages/password-reseted-page/password-reseted-page.component';
import { ForgotPasswordEmailPageComponent } from './pages/forgot-password-email-page/forgot-password-email-page.component';
import { resetPasswordGuard } from './auth/reset-password.guard';

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
];
