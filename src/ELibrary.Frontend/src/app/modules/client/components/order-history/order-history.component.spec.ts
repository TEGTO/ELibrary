/* eslint-disable @typescript-eslint/no-explicit-any */
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatRadioModule } from '@angular/material/radio';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatNativeDateTimeModule, MatTimepickerModule } from '@dhutaryan/ngx-mat-timepicker';
import { of } from 'rxjs';
import { Client, CommandHandler, CurrencyPipeApplier, getDefaultClient, getDefaultOrder, getDefaultOrderBook, Order, OrderStatus, ValidationMessage } from '../../../shared';
import { CLIENT_CANCEL_ORDER_COMMAND_HANDLER, CLIENT_UPDATE_ORDER_COMMAND_HANDLER, ClientCancelOrderCommand, ClientService, ClientUpdateOrderCommand, OrderService } from '../../../shop';
import { OrderHistoryComponent } from './order-history.component';

describe('OrderHistoryComponent', () => {
    let component: OrderHistoryComponent;
    let fixture: ComponentFixture<OrderHistoryComponent>;
    let mockClientService: jasmine.SpyObj<ClientService>;
    let mockOrderService: jasmine.SpyObj<OrderService>;
    let mockUpdateOrderHandler: jasmine.SpyObj<CommandHandler<ClientUpdateOrderCommand>>;
    let mockCancelOrderHandler: jasmine.SpyObj<CommandHandler<ClientCancelOrderCommand>>;

    const mockOrders: Order[] = [
        {
            ...getDefaultOrder(),
            id: 1,
            orderBooks: [
                { ...getDefaultOrderBook(), bookId: 1 }
            ]
        }

    ];
    const mockClient: Client = getDefaultClient();

    beforeEach(async () => {
        const clientServiceSpy = jasmine.createSpyObj('ClientService', ['getClient']);
        const orderServiceSpy = jasmine.createSpyObj('OrderService', ['getOrderTotalAmount', 'getPaginatedOrders']);
        const updateOrderHandlerSpy = jasmine.createSpyObj<CommandHandler<ClientUpdateOrderCommand>>(['dispatch']);
        const cancelOrderHandlerSpy = jasmine.createSpyObj<CommandHandler<ClientCancelOrderCommand>>(['dispatch']);
        const currencyPipeApplierSpy = jasmine.createSpyObj<CurrencyPipeApplier>(['applyCurrencyPipe']);
        const validateInputSpy = jasmine.createSpyObj<ValidationMessage>(['getValidationMessage']);

        validateInputSpy.getValidationMessage.and.returnValue({ hasError: false, message: "" });

        await TestBed.configureTestingModule({
            declarations: [OrderHistoryComponent],
            imports: [
                ReactiveFormsModule,
                MatPaginatorModule,
                BrowserAnimationsModule,
                MatRadioModule,
                MatTimepickerModule,
                MatNativeDateTimeModule,
                MatNativeDateModule,
                MatDatepickerModule
            ],
            providers: [
                { provide: ClientService, useValue: clientServiceSpy },
                { provide: OrderService, useValue: orderServiceSpy },
                { provide: CurrencyPipeApplier, useValue: currencyPipeApplierSpy },
                { provide: ValidationMessage, useValue: validateInputSpy },
                { provide: CLIENT_UPDATE_ORDER_COMMAND_HANDLER, useValue: updateOrderHandlerSpy },
                { provide: CLIENT_CANCEL_ORDER_COMMAND_HANDLER, useValue: cancelOrderHandlerSpy },
            ],
            schemas: [CUSTOM_ELEMENTS_SCHEMA]
        }).compileComponents();

        mockClientService = TestBed.inject(ClientService) as jasmine.SpyObj<ClientService>;
        mockOrderService = TestBed.inject(OrderService) as jasmine.SpyObj<OrderService>;
        mockUpdateOrderHandler = TestBed.inject(CLIENT_UPDATE_ORDER_COMMAND_HANDLER) as jasmine.SpyObj<CommandHandler<ClientUpdateOrderCommand>>;
        mockCancelOrderHandler = TestBed.inject(CLIENT_CANCEL_ORDER_COMMAND_HANDLER) as jasmine.SpyObj<CommandHandler<ClientCancelOrderCommand>>;

        fixture = TestBed.createComponent(OrderHistoryComponent);
        component = fixture.componentInstance;

        mockClientService.getClient.and.returnValue(of(mockClient));
        mockOrderService.getOrderTotalAmount.and.returnValue(of(1));
        mockOrderService.getPaginatedOrders.and.returnValue(of(mockOrders));
    });

    beforeEach(() => {
        fixture.detectChanges();
    });

    it('should create the component', () => {
        expect(component).toBeTruthy();
    });

    it('should initialize client and orders on ngOnInit', () => {
        component.ngOnInit();
        fixture.detectChanges();

        component.client$.subscribe(client => {
            expect(client).toEqual(mockClient);
        });

        component.items$.subscribe(orders => {
            expect(orders).toEqual(mockOrders);
        });
    });

    it('should fetch paginated items when onPageChange is triggered', () => {
        const event: PageEvent = { pageIndex: 1, pageSize: 10, length: 1 };
        spyOn<any>(component, 'fetchPaginatedItems');
        component.onPageChange(event);
        expect(component['fetchPaginatedItems']).toHaveBeenCalledWith({ pageIndex: 2, pageSize: 10 });
    });

    it('should correctly calculate selection size', () => {
        const size = component.calculateSelectionSize(5);
        expect(size).toBe(
            component.scrollSizePerObject * 5 > component.scollSize
                ? component.scollSize
                : component.scrollSizePerObject * 5);
    });

    it('should track by orderBookId', () => {
        const result = component.trackById(0, mockOrders[0].orderBooks[0]);
        expect(result).toBe(1); // bookId
    });

    it('should initialize form for order and disable form if not processing', () => {
        const order = mockOrders[0];
        order.orderStatus = OrderStatus.Canceled;
        component.initializeForm(order);
        const form = component.getFormGroup(order);
        expect(form.disabled).toBe(true);
    });

    it('should call updateOrder with correct form values', () => {
        const order = mockOrders[0];
        component.initializeForm(order);
        const form = component.getFormGroup(order);
        form.patchValue({ address: '456 New St', deliveryTime: new Date() });

        component.updateOrder(order);
        expect(mockUpdateOrderHandler.dispatch).toHaveBeenCalled();
    });

    it('should call cancelOrder on deleteOrder', () => {
        const order = mockOrders[0];
        component.cancelOrder(order);
        expect(mockCancelOrderHandler.dispatch).toHaveBeenCalledWith(jasmine.objectContaining({ order }));
    });
});
