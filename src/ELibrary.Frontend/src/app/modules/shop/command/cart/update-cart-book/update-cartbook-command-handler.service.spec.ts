import { TestBed } from '@angular/core/testing';
import { CartService, UpdateCartBookCommand } from '../../..';
import { CartBook, getDefaultBook, mapCartBookToUpdateCartBookRequest, UpdateCartBookRequest } from '../../../../shared';
import { UpdateCartBookCommandHandlerService } from './update-cartbook-command-handler.service';

describe('UpdateCartBookCommandHandlerService', () => {
    let service: UpdateCartBookCommandHandlerService;
    let cartServiceSpy: jasmine.SpyObj<CartService>;

    beforeEach(() => {
        const spy = jasmine.createSpyObj('CartService', ['updateCartBook']);

        TestBed.configureTestingModule({
            providers: [
                UpdateCartBookCommandHandlerService,
                { provide: CartService, useValue: spy }
            ]
        });

        service = TestBed.inject(UpdateCartBookCommandHandlerService);
        cartServiceSpy = TestBed.inject(CartService) as jasmine.SpyObj<CartService>;
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('dispatch should call cartService.updateCartBook with mapped request', () => {
        const cartBook: CartBook = { id: '1', bookAmount: 1, bookId: 2, book: getDefaultBook() };
        const command: UpdateCartBookCommand = { cartBook };
        const expectedRequest: UpdateCartBookRequest = mapCartBookToUpdateCartBookRequest(cartBook);

        service.dispatch(command);

        expect(cartServiceSpy.updateCartBook).toHaveBeenCalledWith(expectedRequest);
    });
});