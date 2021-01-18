import { Injectable } from '@angular/core';
import { Query } from '@datorama/akita';
import { UserSession } from './user-session.model';
import { UserSessionStore } from './user-session.store';

@Injectable({ providedIn: 'root' })
export class UserSessionQuery extends Query<UserSession> {
  constructor(protected store: UserSessionStore) {
    super(store);
  }
}
