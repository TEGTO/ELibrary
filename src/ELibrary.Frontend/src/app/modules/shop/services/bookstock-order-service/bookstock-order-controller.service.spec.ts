import { TestBed } from '@angular/core/testing';
import { Store } from '@ngrx/store';
import { MockStore, provideMockStore } from '@ngrx/store/testing';
import { of } from 'rxjs';
import { createBookstockOrder, getBookstockOrderAmount, getBookstockOrders, selectBookstockOrders, selectBookstockOrderTotalAmount } from '../..';
import { BookstockApiService, CreateStockBookOrderRequest, getDefaultStockBookOrder, StockBookOrder, StockBookOrderType } from '../../../shared';
import { BookstockOrderControllerService } from './bookstock-order-controller.service';

describe('BookstockOrderControllerService', () => {
    let service: BookstockOrderControllerService;
    let store: MockStore;
    let apiServiceSpy: jasmine.SpyObj<BookstockApiService>;

    beforeEach(() => {
        const apiSpy = jasmine.createSpyObj('BookstockApiService', ['getStockOrderById']);

        TestBed.configureTestingModule({
            providers: [
                BookstockOrderControllerService,
                provideMockStore({}),
                { provide: BookstockApiService, useValue: apiSpy },
            ],
        });

        service = TestBed.inject(BookstockOrderControllerService);
        store = TestBed.inject(Store) as MockStore;
        apiServiceSpy = TestBed.inject(BookstockApiService) as jasmine.SpyObj<BookstockApiService>;
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('getById should call apiService.getStockOrderById with the correct id', () => {
        const mockOrder: StockBookOrder = getDefaultStockBookOrder();
        apiServiceSpy.getStockOrderById.and.returnValue(of(mockOrder));

        service.getById(1).subscribe((order) => {
            expect(order).toEqual(mockOrder);
        });

        expect(apiServiceSpy.getStockOrderById).toHaveBeenCalledWith(1);
    });

    it('getPaginatedOrders should dispatch getBookstockOrders action and select orders from store', () => {
        const mockRequest = { pageNumber: 1, pageSize: 10 };
        const mockOrders: StockBookOrder[] = [
            { ...getDefaultStockBookOrder(), id: 1 },
            { ...getDefaultStockBookOrder(), id: 2 },
        ];
        store.overrideSelector(selectBookstockOrders, mockOrders);
        spyOn(store, 'dispatch');

        service.getPaginatedOrders(mockRequest).subscribe((orders) => {
            expect(orders).toEqual(mockOrders);
        });

        expect(store.dispatch).toHaveBeenCalledWith(getBookstockOrders({ req: mockRequest }));
    });

    it('getOrderTotalAmount should dispatch getBookstockOrderAmount action and select total amount from store', () => {
        const mockTotalAmount = 50;
        store.overrideSelector(selectBookstockOrderTotalAmount, mockTotalAmount);
        spyOn(store, 'dispatch');

        service.getOrderTotalAmount().subscribe((totalAmount) => {
            expect(totalAmount).toBe(mockTotalAmount);
        });

        expect(store.dispatch).toHaveBeenCalledWith(getBookstockOrderAmount());
    });

    it('createStockOrder should dispatch createBookstockOrder action with the correct request', () => {
        const mockRequest: CreateStockBookOrderRequest = {
            type: StockBookOrderType.ClientOrder,
            clientId: 'client1',
            stockBookChanges: [{ bookId: 1, changeAmount: 10 }],
        };
        spyOn(store, 'dispatch');

        service.createStockOrder(mockRequest);
        expect(store.dispatch).toHaveBeenCalledWith(createBookstockOrder({ req: mockRequest }));
    });
});