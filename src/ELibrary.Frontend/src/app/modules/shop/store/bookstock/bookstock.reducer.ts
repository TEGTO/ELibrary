/* eslint-disable @typescript-eslint/no-explicit-any */
import { createReducer, on } from "@ngrx/store";
import { createBookstockOrderFailure, createBookstockOrderSuccess, getBookstockOrderAmountFailure, getBookstockOrderAmountSuccess, getBookstockOrdersFailure, getBookstockOrdersSuccess } from "../..";
import { StockBookOrder } from "../../../shared";

export interface BookstockOrderState {
    orders: StockBookOrder[],
    totalAmount: number,
    error: any
}
const initialBookstockOrderState: BookstockOrderState = {
    orders: [],
    totalAmount: 0,
    error: null
};

export const boockstockOrderReducer = createReducer(
    initialBookstockOrderState,

    on(getBookstockOrdersSuccess, (state, { orders: orders }) => ({
        ...state,
        orders: orders,
        error: null
    })),
    on(getBookstockOrdersFailure, (state, { error: error }) => ({
        ...initialBookstockOrderState,
        error: error
    })),

    on(getBookstockOrderAmountSuccess, (state, { amount: amount }) => ({
        ...state,
        totalAmount: amount,
        error: null
    })),
    on(getBookstockOrderAmountFailure, (state, { error: error }) => ({
        ...initialBookstockOrderState,
        error: error
    })),

    on(createBookstockOrderSuccess, (state, { order: order }) => ({
        ...state,
        orders: [order, ...state.orders],
        totalAmount: state.totalAmount + 1,
        isCreateSuccess: true,
        error: null
    })),
    on(createBookstockOrderFailure, (state, { error: error }) => ({
        ...state,
        error: error
    })),
);