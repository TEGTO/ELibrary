/* eslint-disable @typescript-eslint/no-explicit-any */
import { createReducer, on } from "@ngrx/store";
import { addBookToCartFailure, addBookToCartSuccess, clearCartFailure, clearCartSuccess, deleteCartBookFailure, deleteCartBookSuccess, getCartFailure, getCartSuccess, getInCartAmountFailure, getInCartAmountSuccess, updateCartBookFailure, updateCartBookSuccess } from "../..";
import { CartBook } from "../../../shared";

export interface CartState {
    amount: number,
    books: CartBook[],
    error: any
}
const initialCartState: CartState = {
    amount: 0,
    books: [],
    error: null
};
export const cartReducer = createReducer(
    initialCartState,

    on(getCartSuccess, (state, { cart: cart }) => ({
        ...state,
        amount: cart.books.reduce((total, book) => total + book.bookAmount, 0),
        books: cart.books,
        error: null
    })),
    on(getCartFailure, (state, { error: error }) => ({
        ...initialCartState,
        error: error
    })),

    on(getInCartAmountSuccess, (state, { amount: amount }) => ({
        ...state,
        amount: amount,
        error: null
    })),
    on(getInCartAmountFailure, (state, { error: error }) => ({
        ...initialCartState,
        error: error
    })),

    on(addBookToCartSuccess, (state, { cartBook: cartBook }) => ({
        ...state,
        books: [cartBook, ...state.books],
        amount: state.amount + 1,
        error: null
    })),
    on(addBookToCartFailure, (state, { error: error }) => ({
        ...initialCartState,
        error: error
    })),

    on(updateCartBookSuccess, (state, { cartBook: cartBook }) => {
        const existingCartBook = state.books.find(book => book.id === cartBook.id);
        const updatedBooks = state.books.map(book =>
            book.id === cartBook.id ? { ...book, bookAmount: cartBook.bookAmount } : book
        );
        const amounWithOutOldCartBook = cartBook.bookAmount - (existingCartBook ? existingCartBook.bookAmount : 0);
        return {
            ...state,
            books: updatedBooks,
            amount: amounWithOutOldCartBook + cartBook.bookAmount,
            error: null
        };
    }),
    on(updateCartBookFailure, (state, { error: error }) => ({
        ...initialCartState,
        error: error
    })),

    on(deleteCartBookSuccess, (state, { id: id }) => ({
        ...state,
        books: state.books.filter(x => x.id !== id),
        amount: state.amount - 1,
        error: null
    })),
    on(deleteCartBookFailure, (state, { error: error }) => ({
        ...initialCartState,
        error: error
    })),

    on(clearCartSuccess, () => ({
        ...initialCartState,
    })),
    on(clearCartFailure, (state, { error: error }) => ({
        ...initialCartState,
        error: error
    })),
);