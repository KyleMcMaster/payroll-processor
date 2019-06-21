import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs'
import { map } from 'rxjs/operators';

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'my-app',
  templateUrl: './app.component.html',
  styleUrls: [ './app.component.css' ]
})
export class AppComponent implements OnInit {
  constructor(private http: HttpClient) {}

  myCurrency: currency = { id: "", name: "", market_data: { current_price: { usd: "" }} }
  baseUrl = 'https://api.coingecko.com/api/v3/coins/ethereum'

  ngOnInit() {
    this.get();
  }

  get(): void {
  this.http
    .get<currency>(this.baseUrl)
    .pipe(
      map(r => {
          console.log(r.market_data);
          this.myCurrency = r;
        }),
      ).subscribe();
  }
}

interface currentPrice
{
  usd: string
}

interface marketData
{
  current_price: currentPrice
}

interface currency {
  id: string
  name: string
  market_data: marketData
}
