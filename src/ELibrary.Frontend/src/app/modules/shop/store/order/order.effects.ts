import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, map, mergeMap, of } from "rxjs";
import { cancelOrder, cancelOrderFailure, cancelOrderSuccess, createOrder, createOrderFailure, createOrderSuccess, getOrderAmountFailure, getOrderAmountSuccess, getOrders, getOrdersFailure, getOrdersSuccess, getOrderTotalAmount, managerCancelOrder, managerCancelOrderFailure, managerCancelOrderSuccess, managerGetOrderAmountFailure, managerGetOrderAmountSuccess, managerGetOrderById, managerGetOrderByIdFailure, managerGetOrderByIdSuccess, managerGetOrderTotalAmount, managerGetPaginatedOrders, managerGetPaginatedOrdersFailure, managerGetPaginatedOrdersSuccess, managerUpdateOrder, managerUpdateOrderFailure, managerUpdateOrderSuccess, updateOrder, updateOrderFailure, updateOrderSuccess } from "../..";
import { OrderApiService } from "../../../shared";

@Injectable()
export class OrderEffects {
    constructor(
        private readonly actions$: Actions,
        private readonly apiService: OrderApiService,
    ) { }

    getOrders$ = createEffect(() =>
        this.actions$.pipe(
            ofType(getOrders),
            mergeMap((action) =>
                this.apiService.getPaginatedOrders(action.req).pipe(
                    map(response => getOrdersSuccess({ orders: response })),
                    catchError(error => of(getOrdersFailure({ error: error.message })))
                )
            )
        )
    );

    getOrderAmount$ = createEffect(() =>
        this.actions$.pipe(
            ofType(getOrderTotalAmount),
            mergeMap((action) =>
                this.apiService.getOrderAmount(action.req).pipe(
                    map(response => getOrderAmountSuccess({ amount: response })),
                    catchError(error => of(getOrderAmountFailure({ error: error.message })))
                )
            )
        )
    );

    createOrder$ = createEffect(() =>
        this.actions$.pipe(
            ofType(createOrder),
            mergeMap((action) =>
                this.apiService.createOrder(action.req).pipe(
                    map(response => createOrderSuccess({ order: response })),
                    catchError(error => of(createOrderFailure({ error: error.message })))
                )
            )
        )
    );

    updateOrder$ = createEffect(() =>
        this.actions$.pipe(
            ofType(updateOrder),
            mergeMap((action) =>
                this.apiService.updateOrder(action.req).pipe(
                    map(response => updateOrderSuccess({ order: response })),
                    catchError(error => of(updateOrderFailure({ error: error.message })))
                )
            )
        )
    );

    cancelOrder$ = createEffect(() =>
        this.actions$.pipe(
            ofType(cancelOrder),
            mergeMap((action) =>
                this.apiService.cancelOrder(action.id).pipe(
                    map(() => cancelOrderSuccess({ id: action.id })),
                    catchError(error => of(cancelOrderFailure({ error: error.message })))
                )
            )
        )
    );

    managerGetOrderById$ = createEffect(() =>
        this.actions$.pipe(
            ofType(managerGetOrderById),
            mergeMap((action) =>
                this.apiService.managerGetOrderById(action.id).pipe(
                    map((response) => managerGetOrderByIdSuccess({ order: response })),
                    catchError(error => of(managerGetOrderByIdFailure({ error: error.message })))
                )
            )
        )
    );

    managerGetPaginatedOrders$ = createEffect(() =>
        this.actions$.pipe(
            ofType(managerGetPaginatedOrders),
            mergeMap((action) =>
                this.apiService.managerGetPaginatedOrders(action.req).pipe(
                    map((response) => managerGetPaginatedOrdersSuccess({ orders: response })),
                    catchError(error => of(managerGetPaginatedOrdersFailure({ error: error.message })))
                )
            )
        )
    );

    managerGetOrderAmount$ = createEffect(() =>
        this.actions$.pipe(
            ofType(managerGetOrderTotalAmount),
            mergeMap((action) =>
                this.apiService.managerGetOrderAmount(action.req).pipe(
                    map(response => managerGetOrderAmountSuccess({ amount: response })),
                    catchError(error => of(managerGetOrderAmountFailure({ error: error.message })))
                )
            )
        )
    );

    managerUpdateOrder$ = createEffect(() =>
        this.actions$.pipe(
            ofType(managerUpdateOrder),
            mergeMap((action) =>
                this.apiService.managerUpdateOrder(action.req).pipe(
                    map((response) => managerUpdateOrderSuccess({ order: response })),
                    catchError(error => of(managerUpdateOrderFailure({ error: error.message })))
                )
            )
        )
    );

    managerCancelOrder$ = createEffect(() =>
        this.actions$.pipe(
            ofType(managerCancelOrder),
            mergeMap((action) =>
                this.apiService.managerCancelOrder(action.id).pipe(
                    map(() => managerCancelOrderSuccess({ id: action.id })),
                    catchError(error => of(managerCancelOrderFailure({ error: error.message })))
                )
            )
        )
    );
}