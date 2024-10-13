import { createFeatureSelector, createSelector } from "@ngrx/store";
import { ClientState } from "../..";

export const selectClientState = createFeatureSelector<ClientState>('client');
export const selectClient = createSelector(
    selectClientState,
    (state: ClientState) => state.client
);
export const selectClientErrors = createSelector(
    selectClientState,
    (state: ClientState) => state.error
);