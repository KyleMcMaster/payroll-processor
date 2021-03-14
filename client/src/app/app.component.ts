import {
  Component,
  OnInit,
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { AuthService } from '@auth0/auth0-angular';
import { NotificationService } from '@shared/notification.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  readonly isAuthenticated = this.authService.isAuthenticated$;
  readonly title = 'payroll-processor';

  readonly fragment = this.route.fragment;

  readonly links = [
    { path: 'employees', title: 'Employees' },
    { path: 'payrolls', title: 'Payrolls' },
    { path: 'admin', title: 'Admin' },
  ];

  constructor(
    private readonly authService: AuthService,
    private readonly notificationService: NotificationService,
    private readonly route: ActivatedRoute,
  ) {}

  ngOnInit() {
    this.notificationService.startConnection();
  }
}
