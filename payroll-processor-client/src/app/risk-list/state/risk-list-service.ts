import { Injectable } from "@angular/core";
import { timer } from "rxjs";
import { mapTo } from "rxjs/operators";
import { risks } from "./risk-model";
import { RisksStore } from "./risk-list-store";

@Injectable({ providedIn: "root" })
export class RisksService {
  constructor(private risksStore: RisksStore) {}

  getRisks() {
    timer(1000)
      .pipe(mapTo(risks))
      .subscribe(risks => {
        this.risksStore.set(risks);
      });
  }
}
