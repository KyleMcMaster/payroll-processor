import { Component, OnInit } from '@angular/core';

import { NotificationService } from '@shared/notification.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'payroll-processor';
  active = 'employees';

  constructor(private readonly notificationService: NotificationService) {}

  ngOnInit() {
    this.notificationService.startConnection();
  }
}
