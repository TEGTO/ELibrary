import { NgxMatDatetimePickerModule, NgxMatNativeDateModule, NgxMatTimepickerModule } from '@angular-material-components/datetime-picker';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { CommandHandler, CurrencyPipeApplier, getDefaultOrder, Order, OrderStatus, RouteReader, ValidationMessage } from '../../../../../shared';
import { MANAGER_CANCEL_ORDER_COMMAND_HANDLER, MANAGER_UPDATE_ORDER_COMMAND_HANDLER, ManagerCancelOrderCommand, ManagerUpdateOrderCommand, OrderService } from '../../../../../shop';
import { OrderDetailsComponent } from './order-details.component';

describe('OrderDetailsComponent', () => {
  let component: OrderDetailsComponent;
  let fixture: ComponentFixture<OrderDetailsComponent>;
  let mockOrderService: jasmine.SpyObj<OrderService>;
  let mockCurrencyPipeApplier: jasmine.SpyObj<CurrencyPipeApplier>;
  let mockCancelOrderHandler: jasmine.SpyObj<CommandHandler<ManagerCancelOrderCommand>>;
  let mockUpdateOrderHandler: jasmine.SpyObj<CommandHandler<ManagerUpdateOrderCommand>>;
  let mockRouteReader: jasmine.SpyObj<RouteReader>;
  let mockValidationMessage: jasmine.SpyObj<ValidationMessage>;

  const activatedRouteStub = {
    snapshot: {
      paramMap: {
        // eslint-disable-next-line @typescript-eslint/no-unused-vars
        get: (key: string) => '1'  // Mocking order ID retrieval
      }
    }
  };

  beforeEach(async () => {
    mockOrderService = jasmine.createSpyObj('OrderService', ['managerGetOrderById']);
    mockCurrencyPipeApplier = jasmine.createSpyObj('CurrencyPipeApplier', ['applyCurrencyPipe']);
    mockCancelOrderHandler = jasmine.createSpyObj('CommandHandler', ['dispatch']);
    mockUpdateOrderHandler = jasmine.createSpyObj('CommandHandler', ['dispatch']);
    mockRouteReader = jasmine.createSpyObj<RouteReader>('RouteReader', ['readIdInt']);
    mockValidationMessage = jasmine.createSpyObj<ValidationMessage>('ValidationMessage', ['getValidationMessage']);

    mockRouteReader.readIdInt.and.returnValue(of(getDefaultOrder()));
    mockValidationMessage.getValidationMessage.and.returnValue({ hasError: false, message: "" });

    await TestBed.configureTestingModule({
      declarations: [OrderDetailsComponent],
      imports: [
        ReactiveFormsModule,
        ScrollingModule,
        MatFormFieldModule,
        NgxMatTimepickerModule,
        NgxMatNativeDateModule,
        NgxMatDatetimePickerModule,
        MatInputModule,
        MatSelectModule,
        BrowserAnimationsModule
      ],
      providers: [
        { provide: OrderService, useValue: mockOrderService },
        { provide: CurrencyPipeApplier, useValue: mockCurrencyPipeApplier },
        { provide: ActivatedRoute, useValue: activatedRouteStub },
        { provide: RouteReader, useValue: mockRouteReader },
        { provide: ValidationMessage, useValue: mockValidationMessage },
        { provide: MANAGER_CANCEL_ORDER_COMMAND_HANDLER, useValue: mockCancelOrderHandler },
        { provide: MANAGER_UPDATE_ORDER_COMMAND_HANDLER, useValue: mockUpdateOrderHandler },
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OrderDetailsComponent);
    component = fixture.componentInstance;

    const mockOrder: Order = getDefaultOrder();
    mockOrderService.managerGetOrderById.and.returnValue(of(mockOrder));
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the form with order data', () => {
    const mockOrder: Order = getDefaultOrder();
    const formGroup = component.getFormGroup(mockOrder);

    expect(formGroup).toBeTruthy();
    expect(Math.abs(formGroup.get('deliveryTime')?.value.getTime() - mockOrder.deliveryTime.getTime())).toBeLessThanOrEqual(20);
    expect(formGroup.get('address')?.value).toEqual(mockOrder.deliveryAddress);
    expect(formGroup.get('orderStatus')?.value).toEqual(mockOrder.orderStatus);
  });

  it('should disable the form if the order is canceled', () => {
    const mockOrder: Order = { ...getDefaultOrder(), orderStatus: OrderStatus.Canceled };
    const formGroup = component.getFormGroup(mockOrder);

    expect(formGroup.disabled).toBeTrue();
  });

  it('should disable the form if the order is completed', () => {
    const mockOrder: Order = { ...getDefaultOrder(), orderStatus: OrderStatus.Completed };
    const formGroup = component.getFormGroup(mockOrder);

    expect(formGroup.disabled).toBeTrue();
  });

  it('should call managerCancelOrderHandler when cancelOrder is invoked', () => {
    const mockOrder: Order = getDefaultOrder();
    component.cancelOrder(mockOrder);

    expect(mockCancelOrderHandler.dispatch).toHaveBeenCalledWith(jasmine.objectContaining({
      order: mockOrder
    }));
  });

  it('should call managerUpdateOrderHandler when updateOrder is invoked', () => {
    const mockOrder: Order = getDefaultOrder();
    const formGroup = component.getFormGroup(mockOrder);
    formGroup.get('deliveryTime')?.setValue(new Date());

    component.updateOrder(mockOrder);

    const updatedOrder = { ...mockOrder, deliveryTime: formGroup.get('deliveryTime')?.value };

    expect(mockUpdateOrderHandler.dispatch).toHaveBeenCalledWith(jasmine.objectContaining({
      order: updatedOrder
    }));
  });

  it('should apply currency formatting when applyCurrencyPipe is called', () => {
    component.applyCurrencyPipe(100);
    expect(mockCurrencyPipeApplier.applyCurrencyPipe).toHaveBeenCalledWith(100);
  });
});
