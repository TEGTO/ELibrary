/* eslint-disable @typescript-eslint/no-explicit-any */
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { CreateStockBookOrderRequest, PaginatedRequest, StockBookOrderResponse, URLDefiner } from '../../../..';
import { BookstockApiService } from './bookstock-api.service';

describe('BookstockApiService', () => {
  let service: BookstockApiService;
  let httpTestingController: HttpTestingController;
  let mockUrlDefiner: jasmine.SpyObj<URLDefiner>;

  beforeEach(() => {
    mockUrlDefiner = jasmine.createSpyObj('URLDefiner', ['combineWithShopApiUrl']);
    mockUrlDefiner.combineWithShopApiUrl.and.callFake((subpath: string) => `/api${subpath}`);

    TestBed.configureTestingModule({
      providers: [
        BookstockApiService,
        { provide: URLDefiner, useValue: mockUrlDefiner },
        provideHttpClient(),
        provideHttpClientTesting(),
      ]
    });

    service = TestBed.inject(BookstockApiService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('getStockOrderById should retrieve a stock order by ID', () => {
    const stockOrderId = 1;
    const expectedUrl = `/api/stockbook/${stockOrderId}`;
    const mockResponse: StockBookOrderResponse = {
      id: 1,
      createdAt: new Date(),
      updatedAt: new Date(),
      type: 0,
      totalChangeAmount: 5,
      client: {} as any,
      stockBookChanges: []
    };

    service.getStockOrderById(stockOrderId).subscribe(response => {
      expect(response.id).toBe(1);
      expect(response.totalChangeAmount).toBe(5);
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('getStockOrderAmount should return the total amount of stock orders', () => {
    const expectedUrl = `/api/stockbook/amount`;
    const mockAmount = 42;

    service.getStockOrderAmount().subscribe(amount => {
      expect(amount).toBe(42);
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockAmount);
  });

  it('getStockOrderPaginated should fetch paginated stock orders', () => {
    const request: PaginatedRequest = { pageNumber: 1, pageSize: 10 };
    const expectedUrl = `/api/stockbook/pagination`;
    const mockResponse: StockBookOrderResponse[] = [
      { id: 1, createdAt: new Date(), updatedAt: new Date(), type: 0, totalChangeAmount: 10, client: {} as any, stockBookChanges: [] },
      { id: 2, createdAt: new Date(), updatedAt: new Date(), type: 1, totalChangeAmount: 15, client: {} as any, stockBookChanges: [] }
    ];

    service.getStockOrderPaginated(request).subscribe(response => {
      expect(response.length).toBe(2);
      expect(response[0].totalChangeAmount).toBe(10);
      expect(response[1].id).toBe(2);
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);
  });

  it('createStockBookOrder should post a new stock book order', () => {
    const request: CreateStockBookOrderRequest = {
      type: 0,
      clientId: '123',
      stockBookChanges: [{ bookId: 1, changeAmount: 5 }]
    };
    const expectedUrl = `/api/stockbook`;
    const mockResponse: StockBookOrderResponse = {
      id: 1,
      createdAt: new Date(),
      updatedAt: new Date(),
      type: 0,
      totalChangeAmount: 5,
      client: {} as any,
      stockBookChanges: []
    };

    service.createStockBookOrder(request).subscribe(response => {
      expect(response.id).toBe(1);
      expect(response.totalChangeAmount).toBe(5);
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);
  });

  it('should handle error responses', () => {
    const expectedUrl = `/api/stockbook/amount`;

    service.getStockOrderAmount().subscribe({
      next: () => fail('Expected an error, but got a response'),
      error: (error) => {
        expect(error).toBeTruthy();
      }
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush({ message: 'Internal Server Error' }, { status: 500, statusText: 'Server Error' });
  });
});