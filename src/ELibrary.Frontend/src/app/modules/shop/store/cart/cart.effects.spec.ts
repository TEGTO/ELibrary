/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from "@angular/core/testing";
import { provideMockActions } from "@ngrx/effects/testing";
import { Observable, of, throwError } from "rxjs";
import { addBookToCart, addBookToCartFailure, addBookToCartSuccess, CartEffects, deleteBooksFromCart, deleteBooksFromCartFailure, deleteBooksFromCartSuccess, getCart, getCartFailure, getCartSuccess, getInCartAmount, getInCartAmountFailure, getInCartAmountSuccess, updateCartBook, updateCartBookFailure, updateCartBookSuccess } from "../..";
import { Cart, CartApiService, DeleteCartBookFromCartRequest, getDefaultCart, getDefaultCartBook, UpdateCartBookRequest } from "../../../shared";

describe('CartEffects', () => {
    let actions$: Observable<any>;
    let effects: CartEffects;
    let mockCartApiService: jasmine.SpyObj<CartApiService>;

    const mockCart: Cart = getDefaultCart();

    beforeEach(() => {
        mockCartApiService = jasmine.createSpyObj('CartApiService', [
            'getCart',
            'getInCartCart',
            'addBookToCart',
            'updateCartBookInCart',
            'deleteBooksFromCart'
        ]);

        TestBed.configureTestingModule({
            providers: [
                CartEffects,
                provideMockActions(() => actions$),
                { provide: CartApiService, useValue: mockCartApiService }
            ]
        });

        effects = TestBed.inject(CartEffects);
    });

    describe('getCart$', () => {
        it('should return getCartSuccess on successful cart fetch', (done) => {
            const response = mockCart;
            const action = getCart();
            const outcome = getCartSuccess({ cart: response });

            actions$ = of(action);
            mockCartApiService.getCart.and.returnValue(of(response));

            effects.getCart$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockCartApiService.getCart).toHaveBeenCalled();
                done();
            });
        });

        it('should return getCartFailure on failed cart fetch', (done) => {
            const error = 'Failed to fetch cart';
            const action = getCart();
            const outcome = getCartFailure({ error });

            actions$ = of(action);
            mockCartApiService.getCart.and.returnValue(throwError(() => new Error(error)));

            effects.getCart$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockCartApiService.getCart).toHaveBeenCalled();
                done();
            });
        });
    });

    describe('getInCartAmount$', () => {
        it('should return getInCartAmountSuccess on successful amount fetch', (done) => {
            const amount = 5;
            const action = getInCartAmount();
            const outcome = getInCartAmountSuccess({ amount });

            actions$ = of(action);
            mockCartApiService.getInCartCart.and.returnValue(of(amount));

            effects.getInCartAmount$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockCartApiService.getInCartCart).toHaveBeenCalled();
                done();
            });
        });

        it('should return getInCartAmountFailure on failed amount fetch', (done) => {
            const error = 'Failed to fetch amount';
            const action = getInCartAmount();
            const outcome = getInCartAmountFailure({ error });

            actions$ = of(action);
            mockCartApiService.getInCartCart.and.returnValue(throwError(() => new Error(error)));

            effects.getInCartAmount$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockCartApiService.getInCartCart).toHaveBeenCalled();
                done();
            });
        });
    });

    describe('addBookToCart$', () => {
        it('should return addBookToCartSuccess on successful book addition', (done) => {
            const req = { bookId: 1, bookAmount: 2 };
            const cartBook = getDefaultCartBook();
            const action = addBookToCart({ req });
            const outcome = addBookToCartSuccess({ req, cartBook });

            actions$ = of(action);
            mockCartApiService.addBookToCart.and.returnValue(of(cartBook));

            effects.addBookToCart$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockCartApiService.addBookToCart).toHaveBeenCalledWith(req);
                done();
            });
        });

        it('should return addBookToCartFailure on failed book addition', (done) => {
            const req = { bookId: 1, bookAmount: 2 };
            const error = 'Failed to add book to cart';
            const action = addBookToCart({ req });
            const outcome = addBookToCartFailure({ error });

            actions$ = of(action);
            mockCartApiService.addBookToCart.and.returnValue(throwError(() => new Error(error)));

            effects.addBookToCart$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockCartApiService.addBookToCart).toHaveBeenCalledWith(req);
                done();
            });
        });
    });

    describe('updateCartBook$', () => {
        it('should return updateCartBookSuccess on successful book update', (done) => {
            const req: UpdateCartBookRequest = { id: "1", bookAmount: 5 };
            const cartBook = getDefaultCartBook();
            const action = updateCartBook({ req });
            const outcome = updateCartBookSuccess({ cartBook });

            actions$ = of(action);
            mockCartApiService.updateCartBookInCart.and.returnValue(of(cartBook));

            effects.updateCartBook$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockCartApiService.updateCartBookInCart).toHaveBeenCalledWith(req);
                done();
            });
        });

        it('should return updateCartBookFailure on failed book update', (done) => {
            const req: UpdateCartBookRequest = { id: "1", bookAmount: 5 };
            const error = 'Failed to update cart book';
            const action = updateCartBook({ req });
            const outcome = updateCartBookFailure({ error });

            actions$ = of(action);
            mockCartApiService.updateCartBookInCart.and.returnValue(throwError(() => new Error(error)));

            effects.updateCartBook$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockCartApiService.updateCartBookInCart).toHaveBeenCalledWith(req);
                done();
            });
        });
    });

    describe('deleteBooksFromCart$', () => {
        it('should return deleteBooksFromCartSuccess on successful books deletion', (done) => {
            const requests: DeleteCartBookFromCartRequest[] = [{ id: 1 }];
            const cart = { books: [] };
            const action = deleteBooksFromCart({ requests });
            const outcome = deleteBooksFromCartSuccess({ cart });

            actions$ = of(action);
            mockCartApiService.deleteBooksFromCart.and.returnValue(of(cart));

            effects.deleteBooksFromCart$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockCartApiService.deleteBooksFromCart).toHaveBeenCalledWith(requests);
                done();
            });
        });

        it('should return deleteBooksFromCartFailure on failed books deletion', (done) => {
            const requests: DeleteCartBookFromCartRequest[] = [{ id: 1 }];
            const error = 'Failed to delete books from cart';
            const action = deleteBooksFromCart({ requests });
            const outcome = deleteBooksFromCartFailure({ error });

            actions$ = of(action);
            mockCartApiService.deleteBooksFromCart.and.returnValue(throwError(() => new Error(error)));

            effects.deleteBooksFromCart$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockCartApiService.deleteBooksFromCart).toHaveBeenCalledWith(requests);
                done();
            });
        });
    });
});
