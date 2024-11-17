import { Component, inject, ViewEncapsulation } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { SharedDataService } from '../../../data/services/shared-data.service';

@Component({
  selector: 'app-forgot-password-email-page',
  standalone: true,
  imports: [RouterModule, FormsModule, ReactiveFormsModule],
  templateUrl: './forgot-password-email-page.component.html',
  styleUrls: ['../../../../assets/vendor/css/pages/page-auth.css'],
  encapsulation: ViewEncapsulation.None,
})
export class ForgotPasswordEmailPageComponent {
  email: string = '';

  sharedDataService = inject(SharedDataService);

  ngOnInit(): void {
    this.email = this.sharedDataService.getEmail();
  }
}
