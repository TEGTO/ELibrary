import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { ClientUpdateOrderRequest, CreateOrderRequest, DeliveryMethod, getDefaultOrder, GetOrdersFilter, ManagerUpdateOrderRequest, Order, OrderStatus, PaymentMethod } from '../../../..';
import { URLDefiner } from '../../../url-definer/url-definer.service';
import { OrderApiService } from './order-api.service';

describe('OrderApiService', () => {
  let service: OrderApiService;
  let httpTestingController: HttpTestingController;
  let mockUrlDefiner: jasmine.SpyObj<URLDefiner>;

  beforeEach(() => {
    mockUrlDefiner = jasmine.createSpyObj<URLDefiner>('URLDefiner', ['combineWithShopApiUrl']);
    mockUrlDefiner.combineWithShopApiUrl.and.callFake((subpath: string) => `/api${subpath}`);

    TestBed.configureTestingModule({
      providers: [
        OrderApiService,
        { provide: URLDefiner, useValue: mockUrlDefiner },
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });

    service = TestBed.inject(OrderApiService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get paginated orders', () => {
    const expectedReq = `/api/order/pagination`;
    const request: GetOrdersFilter = { pageNumber: 1, pageSize: 10, clientId: "" };
    const response: Order[] = [getDefaultOrder()];

    service.getPaginatedOrders(request).subscribe(res => {
      expect(res).toEqual(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('POST');
    expect(mockUrlDefiner.combineWithShopApiUrl).toHaveBeenCalledWith('/order/pagination');
    req.flush(response);
  });

  it('should get order amount', () => {
    const expectedReq = `/api/order/amount`;
    const request: GetOrdersFilter = { pageNumber: 1, pageSize: 10, clientId: "" };
    const response = 5;

    service.getOrderAmount(request).subscribe(res => {
      expect(res).toBe(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('POST');
    expect(mockUrlDefiner.combineWithShopApiUrl).toHaveBeenCalledWith('/order/amount');
    req.flush(response);
  });

  it('should create an order', () => {
    const expectedReq = `/api/order`;
    const request: CreateOrderRequest = { contactClientName: "", contactPhone: "", deliveryAddress: "", deliveryTime: new Date(), paymentMethod: PaymentMethod.Cash, deliveryMethod: DeliveryMethod.AddressDelivery, orderBooks: [] };
    const response: Order = getDefaultOrder();

    service.createOrder(request).subscribe(res => {
      expect(res).toEqual(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('POST');
    expect(mockUrlDefiner.combineWithShopApiUrl).toHaveBeenCalledWith('/order');
    req.flush(response);
  });

  it('should update an order', () => {
    const expectedReq = `/api/order`;
    const request: ClientUpdateOrderRequest = { id: 1, contactClientName: "", contactPhone: "", deliveryAddress: "", deliveryTime: new Date(), paymentMethod: PaymentMethod.Cash, deliveryMethod: DeliveryMethod.AddressDelivery };;
    const response: Order = getDefaultOrder();

    service.updateOrder(request).subscribe(res => {
      expect(res).toEqual(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('PATCH');
    expect(mockUrlDefiner.combineWithShopApiUrl).toHaveBeenCalledWith('/order');
    req.flush(response);
  });

  it('should cancel an order', () => {
    const expectedReq = `/api/order/1`;

    service.cancelOrder(1).subscribe(res => {
      expect(res.status).toBe(204);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('DELETE');
    expect(mockUrlDefiner.combineWithShopApiUrl).toHaveBeenCalledWith('/order/1');
    req.flush(null, { status: 204, statusText: 'No Content' });
  });

  it('should handle error on cancel order', () => {
    const expectedReq = `/api/order/1`;

    service.cancelOrder(1).subscribe({
      next: () => fail('Expected an error, not a success'),
      error: (error) => {
        expect(error).toBeTruthy();
      }
    });

    const req = httpTestingController.expectOne(expectedReq);
    req.flush('Error', { status: 400, statusText: 'Bad Request' });
  });

  it('should manager get order by id', () => {
    const expectedReq = `/api/order/manager/1`;
    const response: Order = getDefaultOrder();

    service.managerGetOrderById(1).subscribe(res => {
      expect(res).toEqual(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('GET');
    expect(mockUrlDefiner.combineWithShopApiUrl).toHaveBeenCalledWith('/order/manager/1');
    req.flush(response);
  });

  it('should manager get paginated orders', () => {
    const expectedReq = `/api/order/manager/pagination`;
    const request: GetOrdersFilter = { pageNumber: 1, pageSize: 10, clientId: "" };
    const response: Order[] = [getDefaultOrder()];

    service.managerGetPaginatedOrders(request).subscribe(res => {
      expect(res).toEqual(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('POST');
    expect(mockUrlDefiner.combineWithShopApiUrl).toHaveBeenCalledWith('/order/manager/pagination');
    req.flush(response);
  });

  it('should manager update order', () => {
    const expectedReq = `/api/order/manager`;

    const request: ManagerUpdateOrderRequest = { id: 1, deliveryAddress: 'Shipped', deliveryTime: new Date(), orderStatus: OrderStatus.Canceled };
    const response: Order = getDefaultOrder();

    service.managerUpdateOrder(request).subscribe(res => {
      expect(res).toEqual(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('PUT');
    expect(mockUrlDefiner.combineWithShopApiUrl).toHaveBeenCalledWith('/order/manager');
    req.flush(response);
  });

  it('should handle error on manager update order', () => {
    const expectedReq = `/api/order/manager`;
    const request: ManagerUpdateOrderRequest = { id: 1, deliveryAddress: 'Shipped', deliveryTime: new Date(), orderStatus: OrderStatus.Canceled };

    service.managerUpdateOrder(request).subscribe({
      next: () => fail('Expected an error, not a success'),
      error: (error) => {
        expect(error).toBeTruthy();
      }
    });

    const req = httpTestingController.expectOne(expectedReq);
    req.flush('Error', { status: 400, statusText: 'Bad Request' });
  });
});