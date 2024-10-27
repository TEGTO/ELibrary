/* eslint-disable @typescript-eslint/no-explicit-any */
import { createReducer, on } from "@ngrx/store";
import { cancelOrderFailure, cancelOrderSuccess, createOrder, createOrderFailure, createOrderSuccess, getOrderAmountFailure, getOrderAmountSuccess, getOrdersFailure, getOrdersSuccess, managerCancelOrderFailure, managerCancelOrderSuccess, managerGetOrderAmountFailure, managerGetOrderAmountSuccess, managerGetOrderByIdFailure, managerGetOrderByIdSuccess, managerGetPaginatedOrdersFailure, managerGetPaginatedOrdersSuccess, managerUpdateOrderFailure, managerUpdateOrderSuccess, updateOrderFailure, updateOrderSuccess } from "../..";
import { Order, OrderStatus } from "../../../shared";

//#region Client

export interface OrderState {
    orders: Order[],
    isCreateSuccess: boolean,
    totalAmount: number,
    error: any
}
const initialOrderState: OrderState = {
    orders: [],
    isCreateSuccess: false,
    totalAmount: 0,
    error: null
};

export const orderReducer = createReducer(
    initialOrderState,

    on(getOrdersSuccess, (state, { orders }) => ({
        ...state,
        orders: orders,
        error: null
    })),
    on(getOrdersFailure, (state, { error }) => ({
        ...initialOrderState,
        error: error
    })),

    on(getOrderAmountSuccess, (state, { amount }) => ({
        ...state,
        totalAmount: amount,
        error: null
    })),
    on(getOrderAmountFailure, (state, { error }) => ({
        ...initialOrderState,
        error: error
    })),

    on(createOrder, (state) => ({
        ...state,
        isCreateSuccess: false,
        totalAmount: state.totalAmount + 1,
        error: null
    })),
    on(createOrderSuccess, (state, { order }) => ({
        ...state,
        orders: [order, ...state.orders],
        isCreateSuccess: true,
        error: null
    })),
    on(createOrderFailure, (state, { error }) => ({
        ...state,
        error: error
    })),

    on(updateOrderSuccess, (state, { order }) => (
        {
            ...state,
            orders: state.orders.map(o => o.id === order.id ? order : o),
            error: null
        }
    )),
    on(updateOrderFailure, (state, { error }) => ({
        ...state,
        error: error
    })),

    on(cancelOrderSuccess, (state, { id }) => ({
        ...state,
        orders: state.orders.map(order =>
            order.id === id
                ? { ...order, orderStatus: OrderStatus.Canceled }
                : order
        ),
        error: null
    })),
    on(cancelOrderFailure, (state, { error }) => ({
        ...state,
        error: error
    })),
);

//#endregion


//#region Manager

export interface ManagerOrderState {
    orders: Order[],
    totalAmount: number,
    error: any
}
const initialManagerOrderState: ManagerOrderState = {
    orders: [],
    totalAmount: 0,
    error: null
};

export const managerOrderReducer = createReducer(
    initialManagerOrderState,

    on(managerGetOrderByIdSuccess, (state, { order }) => ({
        ...state,
        orders: [...state.orders, order],
        error: null
    })),
    on(managerGetOrderByIdFailure, (state, { error }) => ({
        ...state,
        error: error
    })),

    on(managerGetPaginatedOrdersSuccess, (state, { orders }) => ({
        ...state,
        orders: orders,
        error: null
    })),
    on(managerGetPaginatedOrdersFailure, (state, { error }) => ({
        ...initialManagerOrderState,
        error: error
    })),

    on(managerGetOrderAmountSuccess, (state, { amount }) => ({
        ...state,
        totalAmount: amount,
        error: null
    })),
    on(managerGetOrderAmountFailure, (state, { error }) => ({
        ...initialOrderState,
        error: error
    })),

    on(managerUpdateOrderSuccess, (state, { order }) => (
        {
            ...state,
            orders: state.orders.map(o => o.id === order.id ? order : o),
            error: null
        }
    )),
    on(managerUpdateOrderFailure, (state, { error }) => ({
        ...state,
        error: error
    })),

    on(managerCancelOrderSuccess, (state, { id }) => (
        {
            ...state,
            orders: state.orders.map(order =>
                order.id === id
                    ? { ...order, orderStatus: OrderStatus.Canceled }
                    : order
            ),
            error: null
        }
    )),
    on(managerCancelOrderFailure, (state, { error }) => ({
        ...state,
        error: error
    })),
);

//#endregion