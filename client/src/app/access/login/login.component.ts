import { Component } from '@angular/core';

import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-login',
  template: `
    <button (click)=loginWithRedirect() class="btn-dark">Log in</button>
  `,
})
export class LoginComponent {
  constructor(private authService: AuthService) {}

  loginWithRedirect() {
    this.authService.loginWithRedirect();
  }
}
