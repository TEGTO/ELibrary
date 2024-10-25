import { TestBed } from '@angular/core/testing';
import { Store } from '@ngrx/store';
import { MockStore, provideMockStore } from '@ngrx/store/testing';
import { addBookToCart, deleteBooksFromCart, getCart, getInCartAmount, selectCartAmount, selectCartBooks, updateCartBook } from '../..';
import { AddBookToCartRequest, CartBook, DeleteCartBookFromCartRequest, getDefaultCartBook, UpdateCartBookRequest } from '../../../shared';
import { CartControllerService } from './cart-controller.service';

describe('CartControllerService', () => {
    let service: CartControllerService;
    let store: MockStore;

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                CartControllerService,
                provideMockStore(),
            ]
        });

        service = TestBed.inject(CartControllerService);
        store = TestBed.inject(Store) as MockStore;
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('getCartBooks should dispatch getCart action and select cart books from store', () => {
        const mockCartBooks: CartBook[] = [
            { ...getDefaultCartBook(), bookId: 1 },
            { ...getDefaultCartBook(), bookId: 2 },
        ];
        store.overrideSelector(selectCartBooks, mockCartBooks);
        spyOn(store, 'dispatch');

        service.getCartBooks().subscribe((books) => {
            expect(books).toEqual(mockCartBooks);
        });

        expect(store.dispatch).toHaveBeenCalledWith(getCart());
    });

    it('getInCartAmount should dispatch getInCartAmount action and select cart amount from store', () => {
        const mockCartAmount = 3;
        store.overrideSelector(selectCartAmount, mockCartAmount);
        spyOn(store, 'dispatch');

        service.getInCartAmount().subscribe((amount) => {
            expect(amount).toBe(mockCartAmount);
        });

        expect(store.dispatch).toHaveBeenCalledWith(getInCartAmount());
    });

    it('addBookToCart should dispatch addBookToCart action with correct request', () => {
        const mockRequest: AddBookToCartRequest = { bookId: 1, bookAmount: 1 };
        spyOn(store, 'dispatch');

        service.addBookToCart(mockRequest);

        expect(store.dispatch).toHaveBeenCalledWith(addBookToCart({ req: mockRequest }));
    });

    it('updateCartBook should dispatch updateCartBook action with correct request', () => {
        const mockRequest: UpdateCartBookRequest = { id: "", bookAmount: 3 };
        spyOn(store, 'dispatch');

        service.updateCartBook(mockRequest);

        expect(store.dispatch).toHaveBeenCalledWith(updateCartBook({ req: mockRequest }));
    });

    it('deleteBooksFromCart should dispatch deleteBooksFromCart action with correct requests', () => {
        const mockRequests: DeleteCartBookFromCartRequest[] = [
            { id: 1 },
            { id: 2 }
        ];
        spyOn(store, 'dispatch');

        service.deleteBooksFromCart(mockRequests);

        expect(store.dispatch).toHaveBeenCalledWith(deleteBooksFromCart({ requests: mockRequests }));
    });
});