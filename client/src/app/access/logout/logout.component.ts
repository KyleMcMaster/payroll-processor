import { DOCUMENT } from '@angular/common';
import {
  Component,
  Inject,
} from '@angular/core';

import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-logout',
  template: `
  <ng-container>
    <button class="btn-dark" (click)="authService.logout({ returnTo: document.location.origin })">
      Log out
    </button>
  </ng-container>
  `,
})
export class LogoutComponent {
  constructor(
    @Inject(DOCUMENT) public document: Document,
    public authService: AuthService,
  ) {}
}
