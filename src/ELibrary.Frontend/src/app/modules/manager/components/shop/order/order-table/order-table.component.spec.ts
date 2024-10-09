/* eslint-disable @typescript-eslint/no-explicit-any */
import { CurrencyPipe, DatePipe } from '@angular/common';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ActivatedRoute, convertToParamMap } from '@angular/router';
import { BehaviorSubject, of } from 'rxjs';
import { CommandHandler, GenericTableComponent, getDefaultOrder, LocaleService, Order } from '../../../../../shared';
import { MANAGER_ORDER_DETAILS_COMMAND_HANDLER, ManagerOrderDetailsCommand, OrderService } from '../../../../../shop';
import { OrderTableComponent } from './order-table.component';

interface OrderItem {
  id: number,
  createdAt: Date,
  totalPrice: number,
  deliveryTime: Date,
  orderStatus: string,
  clientName: string,
  order: Order
}
describe('OrderTableComponent', () => {
  let component: OrderTableComponent;
  let fixture: ComponentFixture<OrderTableComponent>;
  let orderServiceSpy: jasmine.SpyObj<OrderService>;
  let managerOrderDetailsHandlerSpy: jasmine.SpyObj<CommandHandler<ManagerOrderDetailsCommand>>;

  let totalAmount$: BehaviorSubject<number>;

  const mockOrders: Order[] = [
    {
      ...getDefaultOrder(),
      id: 1,
    },
    {
      ...getDefaultOrder(),
      id: 2,
    }
  ];
  const mockOrderItems: OrderItem[] = [
    {
      id: 3,
      createdAt: new Date(),
      totalPrice: 150,
      deliveryTime: new Date(),
      orderStatus: 'Shipped',
      clientName: 'Jane Doe',
      order: {} as Order
    }
  ];


  beforeEach(async () => {
    const orderServiceSpyObj = jasmine.createSpyObj('OrderService', ['managerGetPaginatedOrders', 'getOrderTotalAmount']);
    const localeServiceSpyObj = jasmine.createSpyObj<LocaleService>(['getLocale', 'getCurrency']);
    const managerOrderDetailsHandlerSpyObj = jasmine.createSpyObj('CommandHandler', ['dispatch']);

    localeServiceSpyObj.getCurrency.and.returnValue("en-US");
    localeServiceSpyObj.getLocale.and.returnValue("en-US");

    const activatedRouteStub = new BehaviorSubject(convertToParamMap({ id: '1' }));

    await TestBed.configureTestingModule({
      providers: [
        { provide: ActivatedRoute, useValue: activatedRouteStub },
        { provide: OrderService, useValue: orderServiceSpyObj },
        { provide: LocaleService, useValue: localeServiceSpyObj },
        { provide: MANAGER_ORDER_DETAILS_COMMAND_HANDLER, useValue: managerOrderDetailsHandlerSpyObj },
        CurrencyPipe,
        DatePipe
      ],
      imports: [
        GenericTableComponent,
        OrderTableComponent,
        BrowserAnimationsModule
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(OrderTableComponent);
    component = fixture.componentInstance;
    orderServiceSpy = TestBed.inject(OrderService) as jasmine.SpyObj<OrderService>;
    managerOrderDetailsHandlerSpy = TestBed.inject(MANAGER_ORDER_DETAILS_COMMAND_HANDLER) as jasmine.SpyObj<CommandHandler<any>>;
  });

  beforeEach(() => {
    totalAmount$ = new BehaviorSubject(1);

    orderServiceSpy.managerGetPaginatedOrders.and.returnValue(of(mockOrders));
    orderServiceSpy.getOrderTotalAmount.and.returnValue(totalAmount$.asObservable());

    fixture.detectChanges(); // Initialize component
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch paginated items on initialization', () => {
    expect(orderServiceSpy.managerGetPaginatedOrders).toHaveBeenCalled();
  });

  it('should fetch total amount on initialization', () => {
    expect(orderServiceSpy.getOrderTotalAmount).toHaveBeenCalled();
  });

  it('should render items in the table', () => {
    fixture.detectChanges();
    const rows = fixture.debugElement.queryAll(By.css('app-generic-table'));
    expect(rows.length).toBe(1);
  });

  it('should update order when editItem is triggered', () => {
    const orderItem = mockOrderItems[0];
    component.update(orderItem);

    expect(managerOrderDetailsHandlerSpy.dispatch).toHaveBeenCalledWith(jasmine.objectContaining({
      order: orderItem.order
    }));
  });

  it('should fetch new items when pagination changes', () => {
    spyOn<any>(component, 'fetchPaginatedItems');
    component.onPageChange({ pageIndex: 1, pageSize: 10 });

    expect(component['fetchPaginatedItems']).toHaveBeenCalledWith({ pageIndex: 1, pageSize: 10 });
  });

  it('should display loading table when totalAmount$ is not available', () => {
    totalAmount$.next(0);
    fixture.detectChanges();

    const loadingTable = fixture.debugElement.query(By.css('app-generic-table')).componentInstance;
    expect(loadingTable.totalItemAmount).toBe(0);
  });
});