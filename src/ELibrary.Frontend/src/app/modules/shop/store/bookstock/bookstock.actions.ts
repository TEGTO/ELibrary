/* eslint-disable @typescript-eslint/no-explicit-any */
import { createAction, props } from "@ngrx/store";
import { CreateStockBookOrderRequest, PaginatedRequest, StockBookOrder } from "../../../shared";

export const getBookstockOrders = createAction(
    '[Bookstock Order] Get Bookstock Orders',
    props<{ req: PaginatedRequest }>()
);
export const getBookstockOrdersSuccess = createAction(
    '[Bookstock Order] Get Bookstock Orders Success',
    props<{ orders: StockBookOrder[] }>()
);
export const getBookstockOrdersFailure = createAction(
    '[Bookstock Order] Get Bookstock Orders Failure',
    props<{ error: any }>()
);

export const getBookstockOrderAmount = createAction(
    '[Bookstock Order] Get Bookstock Order Amount'
);
export const getBookstockOrderAmountSuccess = createAction(
    '[Bookstock Order] Get Bookstock Orders Amount Success',
    props<{ amount: number }>()
);
export const getBookstockOrderAmountFailure = createAction(
    '[Bookstock Order] Get Bookstock Orders Amount Failure',
    props<{ error: any }>()
);

export const createBookstockOrder = createAction(
    '[Bookstock Order] Create Bookstock Order',
    props<{ req: CreateStockBookOrderRequest }>()
);
export const createBookstockOrderSuccess = createAction(
    '[Bookstock Order] Create Bookstock Order Success',
    props<{ order: StockBookOrder }>()
);
export const createBookstockOrderFailure = createAction(
    '[Bookstock Order] Create Bookstock Order Failure',
    props<{ error: any }>()
);