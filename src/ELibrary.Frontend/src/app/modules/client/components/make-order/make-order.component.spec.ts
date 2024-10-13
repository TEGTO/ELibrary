import { NgxMatDatetimePickerModule, NgxMatNativeDateModule, NgxMatTimepickerModule } from '@angular-material-components/datetime-picker';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, fakeAsync, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatRadioModule } from '@angular/material/radio';
import { By } from '@angular/platform-browser';
import { of } from 'rxjs';
import { CartBook, Client, CommandHandler, CurrencyPipeApplier, getDefaultBook, getDefaultClient, PaymentMethod, ValidationMessage } from '../../../shared';
import { CartService, CLIENT_ADD_ORDER_COMMAND_HANDLER, ClientAddOrderCommand, ClientService } from '../../../shop';
import { MakeOrderComponent } from './make-order.component';

describe('MakeOrderComponent', () => {
    let component: MakeOrderComponent;
    let fixture: ComponentFixture<MakeOrderComponent>;
    let mockCartService: jasmine.SpyObj<CartService>;
    let mockClientService: jasmine.SpyObj<ClientService>;
    let mockAddOrderHandler: jasmine.SpyObj<CommandHandler<ClientAddOrderCommand>>;
    let mockCurrencyApplier: jasmine.SpyObj<CurrencyPipeApplier>;

    const mockClient: Client = { ...getDefaultClient(), id: "1", address: '123 Main St', name: 'John Doe' };
    const mockCartBooks: CartBook[] = [
        { id: "1", bookId: 1, book: { ...getDefaultBook(), price: 10, stockAmount: 10 }, bookAmount: 2, },
        { id: "1", bookId: 2, book: { ...getDefaultBook(), price: 15, stockAmount: 10 }, bookAmount: 1 }
    ];

    beforeEach(async () => {
        const cartServiceSpy = jasmine.createSpyObj('CartService', ['getCartBooks']);
        const clientServiceSpy = jasmine.createSpyObj('ClientService', ['getClient']);
        const addOrderHandlerSpy = jasmine.createSpyObj('CommandHandler', ['dispatch']);
        const currencyApplierSpy = jasmine.createSpyObj('CurrencyPipeApplier', ['applyCurrencyPipe']);
        const validateInput = jasmine.createSpyObj<ValidationMessage>(['getValidationMessage']);

        validateInput.getValidationMessage.and.returnValue({ hasError: false, message: '' });

        await TestBed.configureTestingModule({
            declarations: [MakeOrderComponent],
            imports: [
                ReactiveFormsModule,
                MatDialogModule,
                NgxMatTimepickerModule,
                NgxMatNativeDateModule,
                NgxMatDatetimePickerModule,
                MatRadioModule
            ],
            providers: [
                { provide: CartService, useValue: cartServiceSpy },
                { provide: ClientService, useValue: clientServiceSpy },
                { provide: CLIENT_ADD_ORDER_COMMAND_HANDLER, useValue: addOrderHandlerSpy },
                { provide: CurrencyPipeApplier, useValue: currencyApplierSpy },
                { provide: ValidationMessage, useValue: validateInput },
            ],
            schemas: [CUSTOM_ELEMENTS_SCHEMA]
        }).compileComponents();

        fixture = TestBed.createComponent(MakeOrderComponent);
        component = fixture.componentInstance;
        mockCartService = TestBed.inject(CartService) as jasmine.SpyObj<CartService>;
        mockClientService = TestBed.inject(ClientService) as jasmine.SpyObj<ClientService>;
        mockAddOrderHandler = TestBed.inject(CLIENT_ADD_ORDER_COMMAND_HANDLER) as jasmine.SpyObj<CommandHandler<ClientAddOrderCommand>>;
        mockCurrencyApplier = TestBed.inject(CurrencyPipeApplier) as jasmine.SpyObj<CurrencyPipeApplier>;
    });

    beforeEach(() => {
        mockCartService.getCartBooks.and.returnValue(of(mockCartBooks));
        mockClientService.getClient.and.returnValue(of(mockClient));
        fixture.detectChanges();
    });

    it('should create the component', () => {
        expect(component).toBeTruthy();
    });

    it('should initialize form with client data', () => {
        expect(component.formGroup).toBeDefined();
        expect(component.deliveryAddressInput.value).toBe('123 Main St');
        expect(component.paymentMethodInput.value).toBe(PaymentMethod.Cash);
    });

    it('should calculate total price of cart books', () => {
        const totalPrice = component.getTotalPrice(mockCartBooks);
        expect(totalPrice).toBe(35); // 2 * 10 + 1 * 15 = 35
    });

    it('should call makeOrder with the correct order details', fakeAsync(() => {
        spyOn(component, 'makeOrder').and.callThrough();

        const submitButton = fixture.debugElement.query(By.css('button#send-button')).nativeElement;
        submitButton.click();

        fixture.detectChanges();

        expect(component.makeOrder).toHaveBeenCalled();
        expect(mockAddOrderHandler.dispatch).toHaveBeenCalledWith(jasmine.objectContaining({
            order: jasmine.objectContaining({
                deliveryAddress: '123 Main St',
                paymentMethod: PaymentMethod.Cash,
                orderBooks: jasmine.arrayContaining([
                    jasmine.objectContaining({ bookId: 1, bookAmount: 2 }),
                    jasmine.objectContaining({ bookId: 2, bookAmount: 1 })
                ])
            })
        }));
    }));

    it('should apply currency pipe correctly', () => {
        mockCurrencyApplier.applyCurrencyPipe.and.returnValue('$20.00');
        const formattedPrice = component.applyCurrencyPipe(20);
        expect(formattedPrice).toBe('$20.00');
        expect(mockCurrencyApplier.applyCurrencyPipe).toHaveBeenCalledWith(20);
    });
});
