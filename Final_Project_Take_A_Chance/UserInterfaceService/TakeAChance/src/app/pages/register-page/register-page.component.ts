import { Component, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-register-page',
  standalone: true,
  imports: [],
  templateUrl: './register-page.component.html',
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
export class RegisterPageComponent {}
