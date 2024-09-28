import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, map, mergeMap, of } from "rxjs";
import { addBookToCart, addBookToCartFailure, addBookToCartSuccess, deleteBooksFromCart, deleteBooksFromCartFailure, deleteBooksFromCartSuccess, getCart, getCartFailure, getCartSuccess, getInCartAmount, getInCartAmountFailure, getInCartAmountSuccess, updateCartBook, updateCartBookFailure, updateCartBookSuccess } from "../..";
import { CartApiService } from "../../../shared";

@Injectable()
export class CartEffects {
    constructor(
        private readonly actions$: Actions,
        private readonly apiService: CartApiService,
    ) { }

    getCart$ = createEffect(() =>
        this.actions$.pipe(
            ofType(getCart),
            mergeMap(() =>
                this.apiService.getCart().pipe(
                    map(response => getCartSuccess({ cart: response })),
                    catchError(error => of(getCartFailure({ error: error.message })))
                )
            )
        )
    );

    getInCartAmount$ = createEffect(() =>
        this.actions$.pipe(
            ofType(getInCartAmount),
            mergeMap(() =>
                this.apiService.getInCartCart().pipe(
                    map(response => getInCartAmountSuccess({ amount: response })),
                    catchError(error => of(getInCartAmountFailure({ error: error.message })))
                )
            )
        )
    );

    addBookToCart$ = createEffect(() =>
        this.actions$.pipe(
            ofType(addBookToCart),
            mergeMap((action) =>
                this.apiService.addBookToCart(action.req).pipe(
                    map(response => addBookToCartSuccess({ req: action.req, cartBook: response })),
                    catchError(error => of(addBookToCartFailure({ error: error.message })))
                )
            )
        )
    );

    updateCartBook$ = createEffect(() =>
        this.actions$.pipe(
            ofType(updateCartBook),
            mergeMap((action) =>
                this.apiService.updateCartBookInCart(action.req).pipe(
                    map(response => updateCartBookSuccess({ cartBook: response })),
                    catchError(error => of(updateCartBookFailure({ error: error.message })))
                )
            )
        )
    );

    deleteBooksFromCart$ = createEffect(() =>
        this.actions$.pipe(
            ofType(deleteBooksFromCart),
            mergeMap((action) =>
                this.apiService.deleteBooksFromCart(action.requests).pipe(
                    map((response) => deleteBooksFromCartSuccess({ cart: response })),
                    catchError(error => of(deleteBooksFromCartFailure({ error: error.message })))
                )
            )
        )
    );
}