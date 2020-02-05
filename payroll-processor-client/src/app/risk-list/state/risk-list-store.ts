import { Risk, createInitialState } from "./risk-model";
import { EntityState, EntityStore, StoreConfig } from "@datorama/akita";

export interface RisksState extends EntityState<Risk> {}

@StoreConfig({ name: "risks" })
export class RisksStore extends EntityStore<RisksState> {
  constructor() {
    super(createInitialState());
  }
}
