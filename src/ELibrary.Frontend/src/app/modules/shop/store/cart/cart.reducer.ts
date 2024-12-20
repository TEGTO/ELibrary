/* eslint-disable @typescript-eslint/no-explicit-any */
import { createReducer, on } from "@ngrx/store";
import { addBookToCartFailure, addBookToCartSuccess, deleteBooksFromCartFailure, deleteBooksFromCartSuccess, getCartFailure, getCartSuccess, getInCartAmountFailure, getInCartAmountSuccess, updateCartBookFailure, updateCartBookSuccess } from "../..";
import { CartBook } from "../../../shared";

export interface CartState {
    amount: number,
    cartBooks: CartBook[],
    error: any
}
const initialCartState: CartState = {
    amount: 0,
    cartBooks: [],
    error: null
};
export const cartReducer = createReducer(
    initialCartState,

    on(getCartSuccess, (state, { cart }) => ({
        ...state,
        amount: cart.books.reduce((total, book) => total + book.bookAmount, 0),
        cartBooks: cart.books,
        error: null
    })),
    on(getCartFailure, (state, { error }) => ({
        ...initialCartState,
        error: error
    })),

    on(getInCartAmountSuccess, (state, { amount }) => ({
        ...state,
        amount: amount,
        error: null
    })),
    on(getInCartAmountFailure, (state, { error }) => ({
        ...initialCartState,
        error: error
    })),

    on(addBookToCartSuccess, (state, { req, cartBook }) => {
        const updatedBooks = state.cartBooks.find(x => x.id === cartBook.id)
            ? state.cartBooks.map(book =>
                book.id === cartBook.id
                    ? cartBook
                    : book
            )
            : [cartBook, ...state.cartBooks];

        return {
            ...state,
            cartBooks: updatedBooks,
            amount: state.amount + req.bookAmount,
            error: null
        };
    }),
    on(addBookToCartFailure, (state, { error }) => ({
        ...initialCartState,
        error: error
    })),

    on(updateCartBookSuccess, (state, { cartBook }) => {
        const existingCartBook = state.cartBooks.find(book => book.id === cartBook.id);
        const updatedBooks = state.cartBooks.map(book =>
            book.id === cartBook.id ? cartBook : book
        );
        const amounWithOutOldCartBook = existingCartBook
            ? state.amount - existingCartBook.bookAmount + cartBook.bookAmount
            : state.amount + cartBook.bookAmount;
        return {
            ...state,
            cartBooks: updatedBooks,
            amount: amounWithOutOldCartBook,
            error: null
        };
    }),
    on(updateCartBookFailure, (state, { error }) => ({
        ...initialCartState,
        error: error
    })),

    on(deleteBooksFromCartSuccess, (state, { cart }) => ({
        ...state,
        cartBooks: cart.books,
        amount: cart.books.reduce((total, book) => total + book.bookAmount, 0),
        error: null
    })),
    on(deleteBooksFromCartFailure, (state, { error }) => ({
        ...initialCartState,
        error: error
    })),
);