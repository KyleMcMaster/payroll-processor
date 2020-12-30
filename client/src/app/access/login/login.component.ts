import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { IconProp } from '@fortawesome/fontawesome-svg-core';
import { faGithub } from '@fortawesome/free-brands-svg-icons';

import { EnvService } from '@shared/env.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  readonly authenticationUrl: string;

  readonly faGithub = faGithub as IconProp;

  constructor(private envService: EnvService, private router: Router) {
    this.authenticationUrl = this.envService.authenticationUrl;
  }

  ngOnInit(): void {}

  login() {
    this.envService.authenticationUrl;
  }
}
