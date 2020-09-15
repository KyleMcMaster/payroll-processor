import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { NotificationService } from '@shared/notification.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  readonly title = 'payroll-processor';

  readonly fragment = this.route.fragment;

  readonly links = [
    { path: 'employees', title: 'Employees' },
    { path: 'payrolls', title: 'Payrolls' },
    { path: 'admin', title: 'Admin' },
  ];

  constructor(
    private readonly notificationService: NotificationService,
    private readonly route: ActivatedRoute,
  ) {}

  ngOnInit() {
    this.notificationService.startConnection();
  }
}
