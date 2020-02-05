import { Risk, createInitialState } from "./risk-model";
import { EntityState, EntityStore, StoreConfig } from "@datorama/akita";
import { Injectable } from "@angular/core";

export interface RisksState extends EntityState<Risk> {}

@Injectable({ providedIn: "root" })
@StoreConfig({ name: "risks" })
export class RisksStore extends EntityStore<RisksState> {
  constructor() {
    super(createInitialState());
  }
}
