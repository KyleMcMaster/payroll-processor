import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { IconProp } from '@fortawesome/fontawesome-svg-core';
import { faMicrosoft } from '@fortawesome/free-brands-svg-icons';

import { MsalService } from '@azure/msal-angular';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css'],
})
export class LogoutComponent {
  readonly faMicrosoft = faMicrosoft as IconProp;
  readonly isLoggedIn: boolean = false;
  constructor(private authService: MsalService, private router: Router) {
    this.isLoggedIn = !!this.authService.getAccount();
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/']);
  }
}
