import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MsalService } from '@azure/msal-angular';
import { IconProp } from '@fortawesome/fontawesome-svg-core';
import { faMicrosoft } from '@fortawesome/free-brands-svg-icons';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  readonly authenticationUrl: string;

  readonly faMicrosoft = faMicrosoft as IconProp;

  constructor(private authService: MsalService, private router: Router) {}

  ngOnInit(): void {
    const loggedIn = !!this.authService.getAccount();

    if (loggedIn) {
      // console.debug('user is logged in');
      this.router.navigate(['/']);
    }
  }

  login() {
    this.authService.loginPopup();
  }
}
