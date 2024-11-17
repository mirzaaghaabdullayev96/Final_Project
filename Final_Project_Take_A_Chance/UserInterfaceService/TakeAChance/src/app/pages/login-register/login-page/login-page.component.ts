import { Component, inject, signal, ViewEncapsulation } from '@angular/core';
import { AuthService } from '../../../auth/auth.service';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import {
  ErrorFromApi,
  ResultNegative,
} from '../../../data/interfaces/result.interface';

@Component({
  selector: 'app-login-page',
  standalone: true,
  imports: [ReactiveFormsModule, RouterModule],
  templateUrl: './login-page.component.html',
  styleUrls: ['../../../../assets/vendor/css/pages/page-auth.css'],
  encapsulation: ViewEncapsulation.None,
})
export class LoginPageComponent {
  authService = inject(AuthService);
  formBuilder = inject(FormBuilder);
  router = inject(Router);

  error: ErrorFromApi | null = null;

  loginForm: FormGroup = this.formBuilder.group({
    email: this.formBuilder.control('', [
      Validators.required,
      Validators.email,
    ]),
    password: this.formBuilder.control('', [Validators.required]),
  });

  onSubmit() {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      console.log('Form is invalid');
      return;
    }

    if (this.loginForm.valid) {
      this.authService.login(this.loginForm.value).subscribe({
        next: (res) => {
          console.log('Success:', res);
          this.router.navigate(['']);
        },
        error: (err: ResultNegative) => {
          this.error = { message: err.message, propertyName: err.propertyName };
          console.error('Error:', err);
        },
      });
    }
  }

  isPasswordVisible = signal<boolean>(false);
}
