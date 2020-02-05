import { Component, OnInit } from "@angular/core";
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { switchMap, startWith } from 'rxjs/operators';
import { Risk } from './state/risk-model';
import { RiskQuery } from './state/risk-list-query';
import { RisksService } from './state/risk-list-service';

@Component({
  selector: "app-risk-list",
  templateUrl: "./risk-list.component.html",
  styleUrls: ["./risk-list.component.scss"]
})
export class RiskListComponent implements OnInit {
  risks$: Observable<Risk[]>;
  selectLoading$: Observable<boolean>;
  sortControl = new FormControl('tag');

  constructor(
    private risksQuery: RiskQuery,
    private risksService: RisksService
  ) {}

  ngOnInit() {
    this.risks$ = this.sortControl.valueChanges.pipe(
      startWith<keyof Risk>('tag'),
      switchMap(sortBy => {
        return this.risksQuery.selectAll({ sortBy });
      })
    );
    this.selectLoading$ = this.risksQuery.selectLoading();
    this.getRisks();
  }

  getRisks() {
      this.risksService.getRisks();
  }
}
