import { TestBed } from "@angular/core/testing";
import { of } from "rxjs";
import { CartAddBookCommand, CartAddBookCommandHandlerService } from "../../..";
import { AuthenticationDialogManager, AuthenticationService } from "../../../../authentication";
import { getDefaultBook, getDefaultUserAuth } from "../../../../shared";
import { CartService } from "../../../services/cart-service/cart-service";

describe('CartAddBookCommandHandlerService', () => {
    let service: CartAddBookCommandHandlerService;
    let authenticationServiceMock: jasmine.SpyObj<AuthenticationService>;
    let cartServiceMock: jasmine.SpyObj<CartService>;
    let dialogManagerMock: jasmine.SpyObj<AuthenticationDialogManager>;

    beforeEach(() => {
        const authServiceSpy = jasmine.createSpyObj('AuthenticationService', ['getUserAuth']);
        const cartServiceSpy = jasmine.createSpyObj('CartService', ['addBookToCart']);
        const dialogManagerSpy = jasmine.createSpyObj('AuthenticationDialogManager', ['openLoginMenu']);

        TestBed.configureTestingModule({
            providers: [
                CartAddBookCommandHandlerService,
                { provide: AuthenticationService, useValue: authServiceSpy },
                { provide: CartService, useValue: cartServiceSpy },
                { provide: AuthenticationDialogManager, useValue: dialogManagerSpy },
            ]
        });

        service = TestBed.inject(CartAddBookCommandHandlerService);
        authenticationServiceMock = TestBed.inject(AuthenticationService) as jasmine.SpyObj<AuthenticationService>;
        cartServiceMock = TestBed.inject(CartService) as jasmine.SpyObj<CartService>;
        dialogManagerMock = TestBed.inject(AuthenticationDialogManager) as jasmine.SpyObj<AuthenticationDialogManager>;
    });

    it('should call addBookToCart when user is authenticated', () => {
        const command: CartAddBookCommand = { book: getDefaultBook() };
        authenticationServiceMock.getUserAuth.and.returnValue(of({ ...getDefaultUserAuth(), isAuthenticated: true }));

        service.dispatch(command);

        expect(cartServiceMock.addBookToCart).toHaveBeenCalledWith(jasmine.objectContaining({ bookId: 0 }));
        expect(dialogManagerMock.openLoginMenu).not.toHaveBeenCalled();
    });

    it('should open login menu when user is not authenticated', () => {
        const command: CartAddBookCommand = { book: getDefaultBook() };
        authenticationServiceMock.getUserAuth.and.returnValue(of({ ...getDefaultUserAuth(), isAuthenticated: false }));

        service.dispatch(command);

        expect(dialogManagerMock.openLoginMenu).toHaveBeenCalled();
        expect(cartServiceMock.addBookToCart).not.toHaveBeenCalled();
    });
});