import { Component, ViewEncapsulation } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-email-verified-page',
  standalone: true,
  imports: [RouterModule, FormsModule, ReactiveFormsModule],
  templateUrl: './email-verified-page.component.html',
  styleUrls: ['../../../../assets/vendor/css/pages/page-auth.css'],
  encapsulation: ViewEncapsulation.None,
})
export class EmailVerifiedPageComponent {}
