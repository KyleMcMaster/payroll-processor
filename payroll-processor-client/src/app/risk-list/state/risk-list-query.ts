import { Injectable } from "@angular/core";
import { QueryEntity } from "@datorama/akita";
import { RisksState, RisksStore } from "./risk-list-store";
import { Risk } from "./risk-model";

@Injectable({ providedIn: "root" })
export class RiskQuery extends QueryEntity<RisksState, Risk> {
  constructor(protected store: RisksStore) {
    super(store);
  }
}
