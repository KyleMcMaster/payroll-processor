import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { IconProp } from '@fortawesome/fontawesome-svg-core';
import { faMicrosoft } from '@fortawesome/free-brands-svg-icons';

import { MsalService } from '@azure/msal-angular';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  readonly faMicrosoft = faMicrosoft as IconProp;

  constructor(private authService: MsalService, private router: Router) {}

  ngOnInit(): void {
    const loggedIn = !!this.authService.getAccount();

    if (loggedIn) {
      this.router.navigate(['/employees']);
    }
  }

  login() {
    this.authService.loginRedirect();
    this.router.navigate(['/employees']);
  }
}
