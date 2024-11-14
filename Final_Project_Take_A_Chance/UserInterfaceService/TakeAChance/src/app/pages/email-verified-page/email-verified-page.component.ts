import { Component, ViewEncapsulation } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-email-verified-page',
  standalone: true,
  imports: [RouterModule, FormsModule, ReactiveFormsModule],
  templateUrl: './email-verified-page.component.html',
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
export class EmailVerifiedPageComponent {

}
