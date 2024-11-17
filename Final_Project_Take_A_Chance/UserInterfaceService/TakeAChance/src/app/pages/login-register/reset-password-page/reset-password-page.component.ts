import {
  Component,
  inject,
  OnInit,
  signal,
  ViewEncapsulation,
} from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import {
  AuthService,
  RegisterRequest,
  ResetPasswordRequest,
} from '../../../auth/auth.service';
import {
  ErrorFromApi,
  ResultNegative,
} from '../../../data/interfaces/result.interface';

@Component({
  selector: 'app-reset-password-page',
  standalone: true,
  imports: [RouterModule, FormsModule, ReactiveFormsModule],
  templateUrl: './reset-password-page.component.html',
  styleUrls: ['../../../../assets/vendor/css/pages/page-auth.css'],
  encapsulation: ViewEncapsulation.None,
})
export class ResetPasswordPageComponent implements OnInit {
  authService = inject(AuthService);
  formBuilder = inject(FormBuilder);
  router = inject(Router);

  activeRoute = inject(ActivatedRoute);

  email: string = '';
  token: string = '';

  error: ErrorFromApi | null = null;

  resetPasswordForm: FormGroup = this.formBuilder.group(
    {
      password: this.formBuilder.control('', [
        Validators.required,
        this.passwordValidator,
      ]),
      confirmpassword: this.formBuilder.control('', [Validators.required]),
    },
    { validators: this.passwordMatchValidator }
  );

  passwordValidator(control: AbstractControl): ValidationErrors | null {
    const password: string = control.value || '';

    const hasUpperCase = /[A-Z]/.test(password);
    const hasLowerCase = /[a-z]/.test(password);
    const hasDigit = /\d/.test(password);
    const hasNonAlphanumeric = /[^a-zA-Z0-9]/.test(password);
    const hasMinLength = password.length >= 8;
    const hasUniqueChars = new Set(password).size >= 1;

    const errors: ValidationErrors = {};
    if (
      !hasUpperCase ||
      !hasLowerCase ||
      !hasDigit ||
      !hasNonAlphanumeric ||
      !hasMinLength ||
      !hasUniqueChars
    )
      errors['notValidPassword'] = true;
    return Object.keys(errors).length ? errors : null;
  }

  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password')?.value;
    const confirmpassword = control.get('confirmpassword')?.value;
    return password === confirmpassword ? null : { passwordMismatch: true };
  }

  ngOnInit(): void {
    this.email = this.activeRoute.snapshot.queryParamMap.get('email') || '';
    this.token = this.activeRoute.snapshot.queryParamMap.get('token') || '';
  }

  onSubmit() {
    if (this.resetPasswordForm.invalid) {
      this.resetPasswordForm.markAllAsTouched();
      console.log('Form is invalid');
      return;
    }

    const formValue = this.resetPasswordForm.value;

    const payload: ResetPasswordRequest = {
      password: formValue.password,
      confirmpassword: formValue.confirmpassword,
      email: this.email,
      token: this.token,
    };

    if (this.resetPasswordForm.valid) {
      console.log(payload);
      this.authService.resetPassword(payload).subscribe({
        next: (res) => {
          console.log('Success:', res);
          this.router.navigate(['/password-reseted']);
        },
        error: (err: ResultNegative) => {
          this.error = { message: err.message, propertyName: err.propertyName };
          console.log('Error:', this.error);
        },
      });
    }
  }

  isPasswordVisible = signal<boolean>(false);
  isConfirmPasswordVisible = signal<boolean>(false);
}
