/* eslint-disable @typescript-eslint/no-explicit-any */
import { addBookToCartFailure, addBookToCartSuccess, cartReducer, CartState, deleteBooksFromCartFailure, deleteBooksFromCartSuccess, getCartFailure, getCartSuccess, getInCartAmountFailure, getInCartAmountSuccess, updateCartBookFailure, updateCartBookSuccess } from "../..";
import { AddBookToCartRequest, Cart, getDefaultCartBook } from "../../../shared";

describe('CartReducer', () => {
    const initialCartState: CartState = {
        amount: 0,
        cartBooks: [],
        error: null
    };

    it('should return the initial state', () => {
        const state = cartReducer(undefined, { type: '@@INIT' } as any);
        expect(state).toEqual(initialCartState);
    });

    it('should handle getCartSuccess', () => {
        const cart: Cart = {
            books: [
                { ...getDefaultCartBook(), bookAmount: 2 },
                { ...getDefaultCartBook(), bookAmount: 3 }
            ]
        };
        const action = getCartSuccess({ cart });
        const expectedState: CartState = {
            amount: 5,
            cartBooks: cart.books,
            error: null
        };

        const state = cartReducer(initialCartState, action);
        expect(state).toEqual(expectedState);
    });

    it('should handle getCartFailure', () => {
        const error = 'Failed to fetch cart';
        const action = getCartFailure({ error });
        const state = cartReducer(initialCartState, action);

        expect(state.error).toBe(error);
        expect(state.amount).toBe(0);
        expect(state.cartBooks).toEqual([]);
    });

    it('should handle getInCartAmountSuccess', () => {
        const amount = 10;
        const action = getInCartAmountSuccess({ amount });
        const expectedState: CartState = {
            ...initialCartState,
            amount,
            error: null
        };

        const state = cartReducer(initialCartState, action);
        expect(state).toEqual(expectedState);
    });

    it('should handle getInCartAmountFailure', () => {
        const error = 'Failed to fetch amount';
        const action = getInCartAmountFailure({ error });
        const state = cartReducer(initialCartState, action);

        expect(state.error).toBe(error);
        expect(state.amount).toBe(0);
        expect(state.cartBooks).toEqual([]);
    });

    it('should handle addBookToCartSuccess', () => {
        const req: AddBookToCartRequest = { bookId: 0, bookAmount: 2 };
        const cartBook = { ...getDefaultCartBook(), bookAmount: 2 };
        const action = addBookToCartSuccess({ req, cartBook });
        const expectedState: CartState = {
            amount: 2,
            cartBooks: [cartBook],
            error: null
        };

        const state = cartReducer(initialCartState, action);
        expect(state).toEqual(expectedState);
    });

    it('should handle addBookToCartFailure', () => {
        const error = 'Failed to add book to cart';
        const action = addBookToCartFailure({ error });
        const state = cartReducer(initialCartState, action);

        expect(state.error).toBe(error);
        expect(state.amount).toBe(0);
        expect(state.cartBooks).toEqual([]);
    });

    it('should handle updateCartBookSuccess', () => {
        const initialState: CartState = {
            amount: 5,
            cartBooks: [
                { ...getDefaultCartBook(), id: "1", bookAmount: 2 },
                { ...getDefaultCartBook(), id: "2", bookAmount: 3 }
            ],
            error: null
        };

        const cartBook = { ...getDefaultCartBook(), id: "1", bookAmount: 5 };
        const action = updateCartBookSuccess({ cartBook });
        const expectedState: CartState = {
            amount: 8,
            cartBooks: [
                cartBook,
                { ...getDefaultCartBook(), id: "2", bookAmount: 3 }
            ],
            error: null
        };

        const state = cartReducer(initialState, action);
        expect(state).toEqual(jasmine.objectContaining(expectedState));
    });

    it('should handle updateCartBookFailure', () => {
        const error = 'Failed to update book in cart';
        const action = updateCartBookFailure({ error });
        const state = cartReducer(initialCartState, action);

        expect(state.error).toBe(error);
        expect(state.amount).toBe(0);
        expect(state.cartBooks).toEqual([]);
    });

    it('should handle deleteBooksFromCartSuccess', () => {
        const initialState: CartState = {
            amount: 5,
            cartBooks: [
                { ...getDefaultCartBook(), bookAmount: 2 },
                { ...getDefaultCartBook(), bookAmount: 3 }
            ],
            error: null
        };

        const cart = {
            books: [
                { ...getDefaultCartBook(), bookAmount: 3 }
            ]
        };
        const action = deleteBooksFromCartSuccess({ cart });
        const expectedState: CartState = {
            amount: 3,
            cartBooks: cart.books,
            error: null
        };

        const state = cartReducer(initialState, action);
        expect(state).toEqual(expectedState);
    });

    it('should handle deleteBooksFromCartFailure', () => {
        const error = 'Failed to delete books from cart';
        const action = deleteBooksFromCartFailure({ error });
        const state = cartReducer(initialCartState, action);

        expect(state.error).toBe(error);
        expect(state.amount).toBe(0);
        expect(state.cartBooks).toEqual([]);
    });
});