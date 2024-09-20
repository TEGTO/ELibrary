import { createFeatureSelector, createSelector } from "@ngrx/store";
import { CartState } from "../..";

export const selectCartState = createFeatureSelector<CartState>('cart');
export const selectCartAmount = createSelector(
    selectCartState,
    (state: CartState) => state.amount
);
export const selectCartBooks = createSelector(
    selectCartState,
    (state: CartState) => state.books
);
export const selectCartErrors = createSelector(
    selectCartState,
    (state: CartState) => state.error
);