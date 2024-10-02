import { createFeatureSelector, createSelector } from "@ngrx/store";
import { ManagerOrderState, OrderState } from "../..";

export const selectOrderState = createFeatureSelector<OrderState>('order');
export const selectOrders = createSelector(
    selectOrderState,
    (state: OrderState) => state.orders
);
export const selectOrderTotalAmount = createSelector(
    selectOrderState,
    (state: OrderState) => state.totalAmount
);
export const selectIsCreateSuccess = createSelector(
    selectOrderState,
    (state: OrderState) => state.isCreateSuccess
);
export const selectOrderError = createSelector(
    selectOrderState,
    (state: OrderState) => state.error
);
export const selectOrderErrors = createSelector(
    selectOrderState,
    (state: OrderState) => state.error
);

export const selectManagerOrderState = createFeatureSelector<ManagerOrderState>('managerorder');
export const selectManagerOrderById = (orderId: number) => createSelector(
    selectManagerOrderState,
    (state: ManagerOrderState) => state.orders.find(order => order.id === orderId)!
);
export const selectManagerOrders = createSelector(
    selectManagerOrderState,
    (state: ManagerOrderState) => state.orders
);
export const selectManagerOrderTotalAmount = createSelector(
    selectManagerOrderState,
    (state: ManagerOrderState) => state.totalAmount
);
export const selectManagerOrderErrors = createSelector(
    selectManagerOrderState,
    (state: ManagerOrderState) => state.error
);