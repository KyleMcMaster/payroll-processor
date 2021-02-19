import { Injectable } from '@angular/core';
import { Store, StoreConfig } from '@datorama/akita';
import { createInitialState, UserSession } from './user-session.model';

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'user' })
export class UserSessionStore extends Store<UserSession> {
  constructor() {
    super(createInitialState());
  }
}
