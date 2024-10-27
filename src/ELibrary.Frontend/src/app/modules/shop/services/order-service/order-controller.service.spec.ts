import { TestBed } from "@angular/core/testing";
import { Store } from "@ngrx/store";
import { MockStore, provideMockStore } from "@ngrx/store/testing";
import { cancelOrder, createOrder, getOrders, getOrderTotalAmount, managerCancelOrder, managerGetOrderById, managerUpdateOrder, selectIsCreateSuccess, selectManagerOrderById, selectManagerOrderState, selectOrderErrors, selectOrders, selectOrderTotalAmount, updateOrder } from "../..";
import { ClientUpdateOrderRequest, CreateOrderRequest, DeliveryMethod, getDefaultGetOrdersFilter, getDefaultOrder, GetOrdersFilter, ManagerUpdateOrderRequest, Order, OrderStatus, PaymentMethod } from "../../../shared";
import { OrderControllerService } from "./order-controller.service";

describe('OrderControllerService', () => {
    let service: OrderControllerService;
    let store: MockStore;

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                OrderControllerService,
                provideMockStore({})
            ]
        });

        service = TestBed.inject(OrderControllerService);
        store = TestBed.inject(Store) as MockStore;
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('getPaginatedOrders should dispatch getOrders action and return orders', () => {
        const req: GetOrdersFilter = getDefaultGetOrdersFilter();
        const mockOrders: Order[] = [getDefaultOrder()];

        spyOn(store, 'dispatch');
        store.overrideSelector(selectOrders, mockOrders);

        service.getPaginatedOrders(req).subscribe(orders => {
            expect(orders).toEqual(mockOrders);
        });

        expect(store.dispatch).toHaveBeenCalledWith(getOrders({ req }));
    });

    it('getOrderTotalAmount should dispatch getOrderTotalAmount action and return total amount', () => {
        const req: GetOrdersFilter = getDefaultGetOrdersFilter();
        const mockTotal = 42;

        spyOn(store, 'dispatch');
        store.overrideSelector(selectOrderTotalAmount, mockTotal);

        service.getOrderTotalAmount(req).subscribe(total => {
            expect(total).toBe(mockTotal);
        });

        expect(store.dispatch).toHaveBeenCalledWith(getOrderTotalAmount({ req }));
    });

    it('createOrder should dispatch createOrder action and return success status', () => {
        const req: CreateOrderRequest = { deliveryAddress: "", deliveryTime: new Date(), paymentMethod: PaymentMethod.Cash, deliveryMethod: DeliveryMethod.AddressDelivery, orderBooks: [] };
        const mockSuccess = true;
        const mockError = null;

        spyOn(store, 'dispatch');
        store.overrideSelector(selectIsCreateSuccess, mockSuccess);
        store.overrideSelector(selectOrderErrors, mockError);

        service.createOrder(req).subscribe(result => {
            expect(result).toEqual({ isSuccess: true, error: null });
        });

        expect(store.dispatch).toHaveBeenCalledWith(createOrder({ req }));
    });

    it('clientUpdateOrder should dispatch updateOrder action', () => {
        const req: ClientUpdateOrderRequest = { id: 1, deliveryAddress: "", deliveryTime: new Date(), paymentMethod: PaymentMethod.Cash, deliveryMethod: DeliveryMethod.AddressDelivery };

        spyOn(store, 'dispatch');
        service.clientUpdateOrder(req);

        expect(store.dispatch).toHaveBeenCalledWith(updateOrder({ req }));
    });

    it('clientCancelOrder should dispatch cancelOrder action', () => {
        const id = 1;

        spyOn(store, 'dispatch');
        service.clientCancelOrder(id);

        expect(store.dispatch).toHaveBeenCalledWith(cancelOrder({ id }));
    });

    it('managerGetOrderById should dispatch action and return the order', () => {
        const id = 1;
        const mockOrder: Order = { ...getDefaultOrder(), id: id };
        const mockState = { orders: [mockOrder], totalAmount: 0, error: null };

        spyOn(store, 'dispatch');
        store.overrideSelector(selectManagerOrderState, mockState);
        store.overrideSelector(selectManagerOrderById(id), mockOrder);

        service.managerGetOrderById(id).subscribe(order => {
            expect(order).toEqual(mockOrder);
        });

        expect(store.dispatch).toHaveBeenCalledWith(managerGetOrderById({ id }));
    });

    it('managerUpdateOrder should dispatch managerUpdateOrder action', () => {
        const req: ManagerUpdateOrderRequest = { id: 1, deliveryAddress: "", deliveryTime: new Date(), orderStatus: OrderStatus.Canceled };

        spyOn(store, 'dispatch');
        service.managerUpdateOrder(req);

        expect(store.dispatch).toHaveBeenCalledWith(managerUpdateOrder({ req }));
    });

    it('managerCancelOrder should dispatch managerCancelOrder action', () => {
        const id = 1;

        spyOn(store, 'dispatch');
        service.managerCancelOrder(id);

        expect(store.dispatch).toHaveBeenCalledWith(managerCancelOrder({ id }));
    });
});