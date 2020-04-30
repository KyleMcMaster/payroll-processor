import { Injectable } from '@angular/core';

import * as signalR from '@microsoft/signalr';

import { EnvService } from './env.service';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private hubConnection: signalR.HubConnection;
  private readonly apiUrl: string;

  constructor(envService: EnvService) {
    this.apiUrl = envService.env.apiDomain;
  }

  startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${this.apiUrl}/hub/notifications`)
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch((err) => console.log('Error while starting connection: ' + err));

    this.hubConnection.on('received', ({ source, message }) => {
      console.log(source, JSON.parse(message));
    });
  }
}
