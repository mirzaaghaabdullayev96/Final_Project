import { Component, inject, signal, ViewEncapsulation } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService, RegisterRequest } from '../../../auth/auth.service';
import {
  ErrorFromApi,
  ResultNegative,
} from '../../../data/interfaces/result.interface';
import { SharedDataService } from '../../../data/services/shared-data.service';

@Component({
  selector: 'app-register-page',
  standalone: true,
  imports: [RouterModule, FormsModule, ReactiveFormsModule],
  templateUrl: './register-page.component.html',
  styleUrls: ['../../../../assets/vendor/css/pages/page-auth.css'],
  encapsulation: ViewEncapsulation.None,
})
export class RegisterPageComponent {
  authService = inject(AuthService);
  formBuilder = inject(FormBuilder);
  router = inject(Router);
  sharedDataService = inject(SharedDataService);

  error: ErrorFromApi | null = null;

  registerForm: FormGroup = this.formBuilder.group(
    {
      name: this.formBuilder.control('', [Validators.required]),
      surname: this.formBuilder.control(''),
      username: this.formBuilder.control('', [Validators.required]),
      email: this.formBuilder.control('', [
        Validators.required,
        Validators.email,
      ]),
      birthday: this.formBuilder.group(
        {
          day: this.formBuilder.control(null, [Validators.required]),
          month: this.formBuilder.control(null, [Validators.required]),
          year: this.formBuilder.control(null, [Validators.required]),
        },
        { validators: this.birthdayValidator }
      ),
      password: this.formBuilder.control('', [
        Validators.required,
        this.passwordValidator,
      ]),
      confirmpassword: this.formBuilder.control('', [Validators.required]),
    },
    { validators: this.passwordMatchValidator }
  );

  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password')?.value;
    const confirmpassword = control.get('confirmpassword')?.value;
    return password === confirmpassword ? null : { passwordMismatch: true };
  }

  birthdayValidator(group: AbstractControl): ValidationErrors | null {
    const day = group.get('day')?.value;
    const month = group.get('month')?.value;
    const year = group.get('year')?.value;

    if (!day || !month || !year) {
      return null;
    }

    const daysInMonth = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

    if (month === 2) {
      const isLeapYear =
        (year % 4 === 0 && year % 100 !== 0) || year % 400 === 0;
      if (isLeapYear) {
        daysInMonth[1] = 29;
      }
    }

    if (day > daysInMonth[month - 1]) {
      return { invalidDate: true };
    }

    return null;
  }

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

  months = [
    { value: 1, name: 'Jan' },
    { value: 2, name: 'Feb' },
    { value: 3, name: 'Mar' },
    { value: 4, name: 'Apr' },
    { value: 5, name: 'May' },
    { value: 6, name: 'Jun' },
    { value: 7, name: 'Jul' },
    { value: 8, name: 'Aug' },
    { value: 9, name: 'Sep' },
    { value: 10, name: 'Oct' },
    { value: 11, name: 'Nov' },
    { value: 12, name: 'Dec' },
  ];

  days = Array.from({ length: 31 }, (_, i) => i + 1);
  years = Array.from(
    { length: new Date().getFullYear() - 1900 },
    (_, i) => new Date().getFullYear() - i
  );

  onSubmit() {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      console.log('Form is invalid');
      return;
    }

    const formValue = this.registerForm.value;

    const payload: RegisterRequest = {
      name: formValue.name,
      surname: formValue.surname,
      email: formValue.email,
      password: formValue.password,
      username: formValue.username,
      confirmpassword: formValue.confirmpassword,
      birthdate: new Date(
        formValue.birthday.year,
        formValue.birthday.month - 1,
        formValue.birthday.day
      ),
    };

    if (this.registerForm.valid) {
      console.log(payload);
      this.authService.register(payload).subscribe({
        next: (res) => {
          console.log('Success:', res);
          this.sharedDataService.setEmail(payload.email);
          this.router.navigate(['/verify-email']);
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
