/* eslint-disable @typescript-eslint/no-explicit-any */
import { createAction, props } from "@ngrx/store";
import { ClientUpdateOrderRequest, CreateOrderRequest, ManagerUpdateOrderRequest, Order, PaginatedRequest } from "../../../shared";

//#region Client

export const getOrders = createAction(
    '[Order] Get Orders',
    props<{ req: PaginatedRequest }>()
);
export const getOrdersSuccess = createAction(
    '[Order] Get Orders Success',
    props<{ orders: Order[] }>()
);
export const getOrdersFailure = createAction(
    '[Order] Get Orders Failure',
    props<{ error: any }>()
);

export const getOrderTotalAmount = createAction(
    '[Order] Get Total Order Amount'
);
export const getOrderAmountSuccess = createAction(
    '[Order] Get Total Order Amount Success',
    props<{ amount: number }>()
);
export const getOrderAmountFailure = createAction(
    '[Order] Get Total Order Amount Failure',
    props<{ error: any }>()
);

export const createOrder = createAction(
    '[Order] Create Order',
    props<{ req: CreateOrderRequest }>()
);
export const createOrderSuccess = createAction(
    '[Order] Create Order Success',
    props<{ order: Order }>()
);
export const createOrderFailure = createAction(
    '[Order] Create Order Failure',
    props<{ error: any }>()
);

export const updateOrder = createAction(
    '[Order] Update Order',
    props<{ req: ClientUpdateOrderRequest }>()
);
export const updateOrderSuccess = createAction(
    '[Order] Update Order Success',
    props<{ order: Order }>()
);
export const updateOrderFailure = createAction(
    '[Order] Update Order Failure',
    props<{ error: any }>()
);

export const cancelOrder = createAction(
    '[Order] Cancel Order',
    props<{ id: number }>()
);
export const cancelOrderSuccess = createAction(
    '[Order] Cancel Order Success',
    props<{ id: number }>()
);
export const cancelOrderFailure = createAction(
    '[Order] Cancel Order Failure',
    props<{ error: any }>()
);

//#endregion

//#region  Manager

export const managerGetOrderById = createAction(
    '[Order] Manager Get Order By Id',
    props<{ id: number }>()
);
export const managerGetOrderByIdSuccess = createAction(
    '[Order] Manager Get Order By Id Success',
    props<{ order: Order }>()
);
export const managerGetOrderByIdFailure = createAction(
    '[Order] Manager Get Order By Id Failure',
    props<{ error: any }>()
);

export const managerGetPaginatedOrders = createAction(
    '[Order] Manager Get Paginated Orders',
    props<{ req: PaginatedRequest }>()
);
export const managerGetPaginatedOrdersSuccess = createAction(
    '[Order] Manager Get Paginated Orders Success',
    props<{ orders: Order[] }>()
);
export const managerGetPaginatedOrdersFailure = createAction(
    '[Order] Manager Get Paginated Orders Failure',
    props<{ error: any }>()
);

export const managerGetOrderTotalAmount = createAction(
    '[Order] Manager Get Order Total Amount'
);
export const managerGetOrderAmountSuccess = createAction(
    '[Order] Manager Get Order Total Amount Success',
    props<{ amount: number }>()
);
export const managerGetOrderAmountFailure = createAction(
    '[Order] Manager Get Order Total Amount Failure',
    props<{ error: any }>()
);

export const managerUpdateOrder = createAction(
    '[Order] Manager Update Order',
    props<{ req: ManagerUpdateOrderRequest }>()
);
export const managerUpdateOrderSuccess = createAction(
    '[Order] Manager Update Order Success',
    props<{ order: Order }>()
);
export const managerUpdateOrderFailure = createAction(
    '[Order] Manager Update Order Failure',
    props<{ error: any }>()
);

export const managerCancelOrder = createAction(
    '[Order] Manager Cancel Order',
    props<{ id: number }>()
);
export const managerCancelOrderSuccess = createAction(
    '[Order] Cancel Order Success',
    props<{ id: number }>()
);
export const managerCancelOrderFailure = createAction(
    '[Order] Cancel Order Failure',
    props<{ error: any }>()
);

//#endregion