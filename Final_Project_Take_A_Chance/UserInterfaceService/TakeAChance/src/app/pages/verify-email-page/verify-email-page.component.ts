import { Component, inject, OnInit, ViewEncapsulation } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { SharedDataService } from '../../data/services/shared-data.service';

@Component({
  selector: 'app-verify-email-page',
  standalone: true,
  imports: [RouterModule, FormsModule, ReactiveFormsModule],
  templateUrl: './verify-email-page.component.html',
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
export class VerifyEmailPageComponent implements OnInit {
  email: string = '';

  sharedDataService = inject(SharedDataService);

  ngOnInit(): void {
    this.email = this.sharedDataService.getEmail();
  }
}
