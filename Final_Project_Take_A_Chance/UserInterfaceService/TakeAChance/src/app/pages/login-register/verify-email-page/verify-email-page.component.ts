import { Component, inject, OnInit, ViewEncapsulation } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { SharedDataService } from '../../../data/services/shared-data.service';

@Component({
  selector: 'app-verify-email-page',
  standalone: true,
  imports: [RouterModule, FormsModule, ReactiveFormsModule],
  templateUrl: './verify-email-page.component.html',
  styleUrls: ['../../../../assets/vendor/css/pages/page-auth.css'],
  encapsulation: ViewEncapsulation.None,
})
export class VerifyEmailPageComponent implements OnInit {
  email: string = '';

  sharedDataService = inject(SharedDataService);

  ngOnInit(): void {
    this.email = this.sharedDataService.getEmail();
  }
}
