import { ScrollingModule } from '@angular/cdk/scrolling';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { of } from 'rxjs';
import { CartService, CLIENT_START_ADDING_ORDER_COMMAND_HANDLER, ClientStartAddingOrderCommand, DELETE_CART_BOOK_COMMAND_HANDLER, DeleteCartBookCommand, UPDATE_CART_BOOK_COMMAND_HANDLER, UpdateCartBookCommand } from '../../..';
import { Book, CartBook, CommandHandler, CurrencyPipeApplier, getDefaultBook } from '../../../../shared';
import { CartDialogComponent } from './cart-dialog.component';

describe('CartDialogComponent', () => {
    let component: CartDialogComponent;
    let fixture: ComponentFixture<CartDialogComponent>;
    let mockCartService: jasmine.SpyObj<CartService>;
    let mockCurrencyApplier: jasmine.SpyObj<CurrencyPipeApplier>;
    let mockUpdateCartBookHandler: jasmine.SpyObj<CommandHandler<UpdateCartBookCommand>>;
    let mockDeleteCartBookHandler: jasmine.SpyObj<CommandHandler<DeleteCartBookCommand>>;
    let mockStartAddingOrderHandler: jasmine.SpyObj<CommandHandler<ClientStartAddingOrderCommand>>;
    let mockDialogRef: jasmine.SpyObj<MatDialogRef<CartDialogComponent>>;

    beforeEach(async () => {
        mockCartService = jasmine.createSpyObj('CartService', ['getCartBooks']);
        mockCurrencyApplier = jasmine.createSpyObj('CurrencyPipeApplier', ['applyCurrencyPipe']);
        mockUpdateCartBookHandler = jasmine.createSpyObj('CommandHandler', ['dispatch']);
        mockDeleteCartBookHandler = jasmine.createSpyObj('CommandHandler', ['dispatch']);
        mockStartAddingOrderHandler = jasmine.createSpyObj('CommandHandler', ['dispatch']);
        mockDialogRef = jasmine.createSpyObj('MatDialogRef', ['close']);

        await TestBed.configureTestingModule({
            declarations: [CartDialogComponent],
            providers: [
                { provide: CartService, useValue: mockCartService },
                { provide: CurrencyPipeApplier, useValue: mockCurrencyApplier },
                { provide: UPDATE_CART_BOOK_COMMAND_HANDLER, useValue: mockUpdateCartBookHandler },
                { provide: DELETE_CART_BOOK_COMMAND_HANDLER, useValue: mockDeleteCartBookHandler },
                { provide: CLIENT_START_ADDING_ORDER_COMMAND_HANDLER, useValue: mockStartAddingOrderHandler },
                { provide: MatDialogRef, useValue: mockDialogRef },
            ],
            imports: [
                MatDialogModule,
                ScrollingModule,
            ],
            schemas: [CUSTOM_ELEMENTS_SCHEMA]
        })
            .compileComponents();

        fixture = TestBed.createComponent(CartDialogComponent);
        component = fixture.componentInstance;
    });

    it('should create the component', () => {
        expect(component).toBeTruthy();
    });

    it('should initialize cart items from CartService on ngOnInit', () => {
        const mockCartBooks: CartBook[] = [{ id: "", bookAmount: 1, bookId: 1, book: getDefaultBook() }];
        mockCartService.getCartBooks.and.returnValue(of(mockCartBooks));

        component.ngOnInit();
        fixture.detectChanges();

        component.items$.subscribe(items => {
            expect(items).toEqual(mockCartBooks);
        });
    });

    it('should call the currency applier when applying the currency pipe', () => {
        component.applyCurrencyPipe(100);
        expect(mockCurrencyApplier.applyCurrencyPipe).toHaveBeenCalledWith(100);
    });

    it('should calculate selection size correctly', () => {
        expect(component.calculateSelectionSize()).toBe(420);
    });

    it('should dispatch update cart book command on input change', fakeAsync(() => {
        const mockCartBook: CartBook = { id: "", bookAmount: 1, bookId: 1, book: getDefaultBook() };
        const event = { target: { value: '5' } } as unknown as Event;

        component.onInputChange(mockCartBook, event);

        fixture.detectChanges();
        tick(400);

        expect(mockUpdateCartBookHandler.dispatch).toHaveBeenCalled();
    }));

    it('should dispatch delete cart book command when deleteCartBook is called', () => {
        const mockCartBook: CartBook = { id: "", bookAmount: 1, bookId: 1, book: getDefaultBook() };
        component.deleteCartBook(mockCartBook);

        expect(mockDeleteCartBookHandler.dispatch).toHaveBeenCalled();
    });

    it('should dispatch start adding order command when makeOrder is called', () => {
        component.makeOrder();
        expect(mockStartAddingOrderHandler.dispatch).toHaveBeenCalled();
    });

    it('should return false if book is out of stock', () => {
        const book: Book = getDefaultBook();
        expect(component.checkIfInStock(book)).toBeFalse();
    });

    it('should return true if book is in stock', () => {
        const book: Book = { ...getDefaultBook(), stockAmount: 10 };
        expect(component.checkIfInStock(book)).toBeTrue();
    });
});