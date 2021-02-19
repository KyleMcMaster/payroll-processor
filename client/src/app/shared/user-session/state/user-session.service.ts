import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EnvService } from '@shared/env.service';
import { ToastrService } from 'ngx-toastr';
import { UserSession } from './user-session.model';
import { UserSessionStore } from './user-session.store';

@Injectable({ providedIn: 'root' })
export class UserSessionService {
  readonly apiRootUrl: string;

  constructor(
    envService: EnvService,
    private http: HttpClient,
    private store: UserSessionStore,
    private toastr: ToastrService,
  ) {
    this.apiRootUrl = envService.apiRootUrl;
  }

  getUserSession(token: string) {
    this.store.setLoading(true);

    return this.http.get<UserSession>(
      `${this.apiRootUrl}/usersessions/${token}`,
    );
    // .subscribe({
    //   error: () => this.toastr.error(`Could not get user session ${token}`),
    //   next: (userSession) => this.store.update(userSession),
    //   complete: () => this.store.setLoading(false),
    // });
  }
}
