/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpResponse } from "@angular/common/http";
import { TestBed } from "@angular/core/testing";
import { provideMockActions } from "@ngrx/effects/testing";
import { Observable, of, throwError } from "rxjs";
import { cancelOrder, cancelOrderSuccess, createOrder, createOrderSuccess, getOrderAmountSuccess, getOrders, getOrdersFailure, getOrdersSuccess, getOrderTotalAmount, managerCancelOrder, managerCancelOrderSuccess, managerGetOrderById, managerGetOrderByIdSuccess, OrderEffects, updateOrder, updateOrderSuccess } from "../..";
import { getDefaultGetOrdersFilter, getDefaultOrder, Order, OrderApiService } from "../../../shared";

describe('OrderEffects', () => {
    let effects: OrderEffects;
    let apiServiceMock: jasmine.SpyObj<OrderApiService>;
    let actions$: Observable<any>;

    beforeEach(() => {
        apiServiceMock = jasmine.createSpyObj('OrderApiService', [
            'getPaginatedOrders', 'getOrderAmount', 'createOrder',
            'updateOrder', 'cancelOrder', 'managerGetOrderById',
            'managerGetPaginatedOrders', 'managerGetOrderAmount',
            'managerUpdateOrder', 'managerCancelOrder'
        ]);

        TestBed.configureTestingModule({
            providers: [
                OrderEffects,
                provideMockActions(() => actions$),
                { provide: OrderApiService, useValue: apiServiceMock }
            ]
        });

        effects = TestBed.inject(OrderEffects);
    });

    it('should dispatch getOrdersSuccess on successful getOrders API call', (done) => {
        const mockOrders: Order[] = [{ ...getDefaultOrder(), id: 1, orderAmount: 2, totalPrice: 50 }];
        apiServiceMock.getPaginatedOrders.and.returnValue(of(mockOrders));

        actions$ = of(getOrders({ req: getDefaultGetOrdersFilter() }));

        effects.getOrders$.subscribe(action => {
            expect(action).toEqual(getOrdersSuccess({ orders: mockOrders }));
            done();
        });
    });

    it('should dispatch getOrdersFailure on getOrders API error', (done) => {
        const mockError = new Error('API Error');
        apiServiceMock.getPaginatedOrders.and.returnValue(throwError(() => mockError));

        actions$ = of(getOrders({ req: getDefaultGetOrdersFilter() }));

        effects.getOrders$.subscribe(action => {
            expect(action).toEqual(getOrdersFailure({ error: mockError.message }));
            done();
        });
    });

    it('should dispatch getOrderAmountSuccess on successful getOrderAmount API call', (done) => {
        apiServiceMock.getOrderAmount.and.returnValue(of(42));

        actions$ = of(getOrderTotalAmount({ req: getDefaultGetOrdersFilter() }));

        effects.getOrderAmount$.subscribe(action => {
            expect(action).toEqual(getOrderAmountSuccess({ amount: 42 }));
            done();
        });
    });

    it('should dispatch createOrderSuccess on successful createOrder API call', (done) => {
        const mockOrder: Order = { ...getDefaultOrder(), id: 1, orderAmount: 2, totalPrice: 50 };
        apiServiceMock.createOrder.and.returnValue(of(mockOrder));

        actions$ = of(createOrder({ req: mockOrder }));

        effects.createOrder$.subscribe(action => {
            expect(action).toEqual(createOrderSuccess({ order: mockOrder }));
            done();
        });
    });

    it('should dispatch updateOrderSuccess on successful updateOrder API call', (done) => {
        const updatedOrder: Order = { ...getDefaultOrder(), id: 1, orderAmount: 3, totalPrice: 60 };
        apiServiceMock.updateOrder.and.returnValue(of(updatedOrder));

        actions$ = of(updateOrder({ req: updatedOrder }));

        effects.updateOrder$.subscribe(action => {
            expect(action).toEqual(updateOrderSuccess({ order: updatedOrder }));
            done();
        });
    });

    it('should dispatch cancelOrderSuccess on successful cancelOrder API call', (done) => {
        apiServiceMock.cancelOrder.and.returnValue(of(new HttpResponse<void>()));

        actions$ = of(cancelOrder({ id: 1 }));

        effects.cancelOrder$.subscribe(action => {
            expect(action).toEqual(cancelOrderSuccess({ id: 1 }));
            done();
        });
    });

    it('should dispatch managerGetOrderByIdSuccess on successful getOrderById API call', (done) => {
        const mockOrder: Order = { ...getDefaultOrder(), id: 1, orderAmount: 2, totalPrice: 50 };
        apiServiceMock.managerGetOrderById.and.returnValue(of(mockOrder));

        actions$ = of(managerGetOrderById({ id: 1 }));

        effects.managerGetOrderById$.subscribe(action => {
            expect(action).toEqual(managerGetOrderByIdSuccess({ order: mockOrder }));
            done();
        });
    });

    it('should dispatch managerCancelOrderSuccess on successful managerCancelOrder API call', (done) => {
        apiServiceMock.managerCancelOrder.and.returnValue(of(new HttpResponse<void>()));

        actions$ = of(managerCancelOrder({ id: 1 }));

        effects.managerCancelOrder$.subscribe(action => {
            expect(action).toEqual(managerCancelOrderSuccess({ id: 1 }));
            done();
        });
    });
});