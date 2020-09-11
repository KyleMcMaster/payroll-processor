import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ClockService {
  now(): Date {
    return new Date();
  }
}
