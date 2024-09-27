import { createFeatureSelector, createSelector } from "@ngrx/store";
import { BookstockOrderState } from "../..";

export const selectBookstockOrderState = createFeatureSelector<BookstockOrderState>('bookstockorder');
export const selectBookstockOrders = createSelector(
    selectBookstockOrderState,
    (state: BookstockOrderState) => state.orders
);
export const selectBookstockOrderTotalAmount = createSelector(
    selectBookstockOrderState,
    (state: BookstockOrderState) => state.totalAmount
);
export const selectBookstockError = createSelector(
    selectBookstockOrderState,
    (state: BookstockOrderState) => state.error
);