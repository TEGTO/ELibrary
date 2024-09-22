/* eslint-disable @typescript-eslint/no-explicit-any */
import { createAction, props } from "@ngrx/store";
import { AddBookToCartRequest, Cart, CartBook, UpdateCartBookRequest } from "../../../shared";

export const getCart = createAction(
    '[Cart] Get Cart'
);
export const getCartSuccess = createAction(
    '[Cart] Get Cart Success',
    props<{ cart: Cart }>()
);
export const getCartFailure = createAction(
    '[Cart] Get Cart Failure',
    props<{ error: any }>()
);

export const getInCartAmount = createAction(
    '[Cart] Get In Cart Amount',
);
export const getInCartAmountSuccess = createAction(
    '[Cart] Get In Cart Amount Success',
    props<{ amount: number }>()
);
export const getInCartAmountFailure = createAction(
    '[Cart] Get In Cart Amount Failure',
    props<{ error: any }>()
);

export const addBookToCart = createAction(
    '[Cart] Add Book To Cart',
    props<{ req: AddBookToCartRequest }>()
);
export const addBookToCartSuccess = createAction(
    '[Cart] Add Book To Cart Success',
    props<{ req: AddBookToCartRequest, cartBook: CartBook }>()
);
export const addBookToCartFailure = createAction(
    '[Cart] Add Book To Cart Failure',
    props<{ error: any }>()
);

export const updateCartBook = createAction(
    '[Cart] Update Cart Book',
    props<{ req: UpdateCartBookRequest }>()
);
export const updateCartBookSuccess = createAction(
    '[Cart] Update Cart Book Success',
    props<{ cartBook: CartBook }>()
);
export const updateCartBookFailure = createAction(
    '[Cart] Update Cart Book Failure',
    props<{ error: any }>()
);

export const deleteCartBook = createAction(
    '[Cart] Delete Cart Book',
    props<{ id: string }>()
);
export const deleteCartBookSuccess = createAction(
    '[Cart] Delete Cart Book Success',
    props<{ id: string }>()
);
export const deleteCartBookFailure = createAction(
    '[Cart] Delete Cart Book Failure',
    props<{ error: any }>()
);

export const clearCart = createAction(
    '[Cart] Clear Cart'
);
export const clearCartSuccess = createAction(
    '[Cart] Clear Cart Success'
);
export const clearCartFailure = createAction(
    '[Cart] Clear Cart Failure',
    props<{ error: any }>()
);