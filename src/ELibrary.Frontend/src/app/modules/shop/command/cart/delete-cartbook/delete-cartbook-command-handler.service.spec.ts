import { TestBed } from '@angular/core/testing';
import { CartService, DeleteCartBookCommand } from '../../..';
import { CartBook, getDefaultBook, mapCartBookToDeleteCartBookFromCartRequest } from '../../../../shared';
import { DeleteCartBookCommandHandlerService } from './delete-cartbook-command-handler.service';

describe('DeleteCartBookCommandHandlerService', () => {
    let service: DeleteCartBookCommandHandlerService;
    let cartServiceSpy: jasmine.SpyObj<CartService>;

    beforeEach(() => {
        cartServiceSpy = jasmine.createSpyObj('CartService', ['deleteBooksFromCart']);
        TestBed.configureTestingModule({
            providers: [
                DeleteCartBookCommandHandlerService,
                { provide: CartService, useValue: cartServiceSpy }
            ]
        });

        service = TestBed.inject(DeleteCartBookCommandHandlerService);
    });

    it('should create the service', () => {
        expect(service).toBeTruthy();
    });

    it('should dispatch delete cart book command and call deleteBooksFromCart with mapped requests', () => {
        const cartBook: CartBook = {
            id: '1',
            bookAmount: 1,
            bookId: 1,
            book: getDefaultBook()
        };
        const command: DeleteCartBookCommand = { cartBook };
        const mappedRequest = mapCartBookToDeleteCartBookFromCartRequest(cartBook);

        service.dispatch(command);

        expect(cartServiceSpy.deleteBooksFromCart).toHaveBeenCalledWith([mappedRequest]);
    });
});