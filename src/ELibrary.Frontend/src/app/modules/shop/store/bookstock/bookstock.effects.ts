import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, map, mergeMap, of } from "rxjs";
import { createBookstockOrder, createBookstockOrderFailure, createBookstockOrderSuccess, getBookstockOrderAmount, getBookstockOrderAmountFailure, getBookstockOrderAmountSuccess, getBookstockOrders, getBookstockOrdersFailure, getBookstockOrdersSuccess } from "../..";
import { BookstockApiService } from "../../../shared";

@Injectable()
export class BookstockEffects {
    constructor(
        private readonly actions$: Actions,
        private readonly apiService: BookstockApiService,
    ) { }

    getBookstockOrders$ = createEffect(() =>
        this.actions$.pipe(
            ofType(getBookstockOrders),
            mergeMap((action) =>
                this.apiService.getStockOrderPaginated(action.req).pipe(
                    map(response => getBookstockOrdersSuccess({ orders: response })),
                    catchError(error => of(getBookstockOrdersFailure({ error: error.message })))
                )
            )
        )
    );

    getBookstockOrderAmount$ = createEffect(() =>
        this.actions$.pipe(
            ofType(getBookstockOrderAmount),
            mergeMap(() =>
                this.apiService.getStockOrderAmount().pipe(
                    map(response => getBookstockOrderAmountSuccess({ amount: response })),
                    catchError(error => of(getBookstockOrderAmountFailure({ error: error.message })))
                )
            )
        )
    );

    createOrder$ = createEffect(() =>
        this.actions$.pipe(
            ofType(createBookstockOrder),
            mergeMap((action) =>
                this.apiService.createStockBookOrder(action.req).pipe(
                    map(response => createBookstockOrderSuccess({ order: response })),
                    catchError(error => of(createBookstockOrderFailure({ error: error.message })))
                )
            )
        )
    );
}