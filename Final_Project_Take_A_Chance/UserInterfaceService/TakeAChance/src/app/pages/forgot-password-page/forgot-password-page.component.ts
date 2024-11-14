import { Component, inject, ViewEncapsulation } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../auth/auth.service';
import {
  ErrorFromApi,
  ResultNegative,
} from '../../data/interfaces/result.interface';
import { SharedDataService } from '../../data/services/shared-data.service';

@Component({
  selector: 'app-forgot-password-page',
  standalone: true,
  imports: [RouterModule, FormsModule, ReactiveFormsModule],
  templateUrl: './forgot-password-page.component.html',
  styleUrls: [
    '../../../assets/LoginAndRegister/vendor/fonts/fontawesome.css',
    '../../../assets/LoginAndRegister/vendor/fonts/tabler-icons.css',
    '../../../assets/LoginAndRegister/vendor/css/rtl/core.css',
    '../../../assets/LoginAndRegister/vendor/css/rtl/theme-default.css',
    '../../../assets/LoginAndRegister/css/demo.css',
    '../../../assets/LoginAndRegister/vendor/libs/node-waves/node-waves.css',
    '../../../assets/LoginAndRegister/vendor/libs/perfect-scrollbar/perfect-scrollbar.css',
    '../../../assets/LoginAndRegister/vendor/libs/typeahead-js/typeahead.css',
    '../../../assets/LoginAndRegister/vendor/css/pages/page-auth.css',
  ],
  encapsulation: ViewEncapsulation.None,
})
export class ForgotPasswordPageComponent {
  authService = inject(AuthService);
  formBuilder = inject(FormBuilder);
  router = inject(Router);
  sharedDataService = inject(SharedDataService);
  
  error: ErrorFromApi | null = null;

  forgotpassForm: FormGroup = this.formBuilder.group({
    email: this.formBuilder.control('', [
      Validators.required,
      Validators.email,
    ]),
  });

  onSubmit() {
    if (this.forgotpassForm.invalid) {
      this.forgotpassForm.markAllAsTouched();
      console.log('Form is invalid');
      return;
    }

    if (this.forgotpassForm.valid) {
      this.authService.forgotPassword(this.forgotpassForm.value).subscribe({
        next: (res) => {
          console.log('Success:', res);
          this.sharedDataService.setEmail(this.forgotpassForm.get('email')?.value);
          this.router.navigate(['/forgot-password-link']);
        },
        error: (err: ResultNegative) => {
          this.error = { message: err.message, propertyName: err.propertyName };
          console.error('Error:', err);
        },
      });
    }
  }
}
