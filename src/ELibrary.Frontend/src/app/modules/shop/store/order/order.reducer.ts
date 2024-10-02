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

    on(getOrdersSuccess, (state, { orders: orders }) => ({
        ...state,
        orders: orders,
        error: null
    })),
    on(getOrdersFailure, (state, { error: error }) => ({
        ...initialOrderState,
        error: error
    })),

    on(getOrderAmountSuccess, (state, { amount: amount }) => ({
        ...state,
        totalAmount: amount,
        error: null
    })),
    on(getOrderAmountFailure, (state, { error: error }) => ({
        ...initialOrderState,
        error: error
    })),

    on(createOrder, (state) => ({
        ...state,
        isCreateSuccess: false,
        error: null
    })),
    on(createOrderSuccess, (state, { order: order }) => ({
        ...state,
        orders: [order, ...state.orders],
        isCreateSuccess: true,
        error: null
    })),
    on(createOrderFailure, (state, { error: error }) => ({
        ...state,
        error: error
    })),

    on(updateOrderSuccess, (state, { order: order }) => (
        {
            ...state,
            orders: state.orders.map(o => o.id === order.id ? order : o),
            error: null
        }
    )),
    on(updateOrderFailure, (state, { error: error }) => ({
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
    on(cancelOrderFailure, (state, { error: error }) => ({
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

    on(managerGetOrderByIdSuccess, (state, { order: order }) => ({
        ...state,
        orders: [...state.orders, order],
        error: null
    })),
    on(managerGetOrderByIdFailure, (state, { error: error }) => ({
        ...state,
        error: error
    })),

    on(managerGetPaginatedOrdersSuccess, (state, { orders: orders }) => ({
        ...state,
        orders: orders,
        error: null
    })),
    on(managerGetPaginatedOrdersFailure, (state, { error: error }) => ({
        ...initialManagerOrderState,
        error: error
    })),

    on(managerGetOrderAmountSuccess, (state, { amount: amount }) => ({
        ...state,
        totalAmount: amount,
        error: null
    })),
    on(managerGetOrderAmountFailure, (state, { error: error }) => ({
        ...initialOrderState,
        error: error
    })),

    on(managerUpdateOrderSuccess, (state, { order: order }) => (
        {
            ...state,
            orders: state.orders.map(o => o.id === order.id ? order : o),
            error: null
        }
    )),
    on(managerUpdateOrderFailure, (state, { error: error }) => ({
        ...state,
        error: error
    })),

    on(managerCancelOrderSuccess, (state, { id: id }) => (
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
    on(managerCancelOrderFailure, (state, { error: error }) => ({
        ...state,
        error: error
    })),
);

//#endregion