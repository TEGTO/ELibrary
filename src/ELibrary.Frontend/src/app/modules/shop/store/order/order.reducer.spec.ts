/* eslint-disable @typescript-eslint/no-explicit-any */
import { cancelOrderSuccess, createOrder, createOrderSuccess, getOrdersFailure, getOrdersSuccess, managerCancelOrderSuccess, managerGetOrderByIdFailure, managerGetOrderByIdSuccess, managerGetPaginatedOrdersSuccess, managerOrderReducer, ManagerOrderState, managerUpdateOrderSuccess, orderReducer, OrderState, updateOrderSuccess } from "../..";
import { getDefaultCreateOrderRequest, getDefaultOrder, Order, OrderStatus } from "../../../shared";

describe('Order Reducer', () => {
    const initialState: OrderState = {
        orders: [],
        isCreateSuccess: false,
        totalAmount: 0,
        error: null,
    };

    it('should return the initial state on unknown action', () => {
        const action = { type: 'UNKNOWN' } as any;
        const state = orderReducer(undefined, action);

        expect(state).toEqual(initialState);
    });

    it('should update orders on getOrdersSuccess', () => {
        const mockOrders: Order[] = [getDefaultOrder()];
        const action = getOrdersSuccess({ orders: mockOrders });

        const state = orderReducer(initialState, action);

        expect(state.orders).toEqual(mockOrders);
        expect(state.error).toBeNull();
    });

    it('should set error on getOrdersFailure', () => {
        const mockError = { message: 'Failed to load orders' };
        const action = getOrdersFailure({ error: mockError });

        const state = orderReducer(initialState, action);

        expect(state.orders).toEqual([]);
        expect(state.error).toEqual(mockError);
    });

    it('should increment totalAmount and set isCreateSuccess to false on createOrder', () => {
        const action = createOrder({ req: getDefaultCreateOrderRequest() });
        const state = orderReducer(initialState, action);

        expect(state.totalAmount).toBe(1);
        expect(state.isCreateSuccess).toBe(false);
    });

    it('should add order and set isCreateSuccess to true on createOrderSuccess', () => {
        const mockOrder: Order = getDefaultOrder();
        const action = createOrderSuccess({ order: mockOrder });

        const state = orderReducer(initialState, action);

        expect(state.orders).toContain(mockOrder);
        expect(state.isCreateSuccess).toBe(true);
    });

    it('should update the order on updateOrderSuccess', () => {
        const initial: OrderState = { ...initialState, orders: [{ ...getDefaultOrder(), id: 1, orderAmount: 2 }] };
        const updatedOrder: Order = { ...getDefaultOrder(), id: 1, orderAmount: 3 };
        const action = updateOrderSuccess({ order: updatedOrder });

        const state = orderReducer(initial, action);

        expect(state.orders[0].orderAmount).toBe(3);
    });

    it('should cancel the order on cancelOrderSuccess', () => {
        const initial = { ...initialState, orders: [{ ...getDefaultOrder(), id: 1, orderStatus: OrderStatus.InProcessing }] };
        const action = cancelOrderSuccess({ id: 1 });

        const state = orderReducer(initial, action);

        expect(state.orders[0].orderStatus).toBe(OrderStatus.Canceled);
    });
});

describe('Manager Order Reducer', () => {
    const initialState: ManagerOrderState = {
        orders: [],
        totalAmount: 0,
        error: null,
    };

    it('should add order on managerGetOrderByIdSuccess', () => {
        const mockOrder: Order = { ...getDefaultOrder(), id: 1, orderAmount: 2, totalPrice: 50 };
        const action = managerGetOrderByIdSuccess({ order: mockOrder });

        const state = managerOrderReducer(initialState, action);

        expect(state.orders).toContain(mockOrder);
    });

    it('should set error on managerGetOrderByIdFailure', () => {
        const mockError = { message: 'Order not found' };
        const action = managerGetOrderByIdFailure({ error: mockError });

        const state = managerOrderReducer(initialState, action);

        expect(state.error).toEqual(mockError);
    });

    it('should replace orders on managerGetPaginatedOrdersSuccess', () => {
        const mockOrders: Order[] = [{ ...getDefaultOrder(), id: 1, orderAmount: 2 }];
        const action = managerGetPaginatedOrdersSuccess({ orders: mockOrders });

        const state = managerOrderReducer(initialState, action);

        expect(state.orders).toEqual(mockOrders);
    });

    it('should update an order on managerUpdateOrderSuccess', () => {
        const initial = { ...initialState, orders: [{ ...getDefaultOrder(), id: 1, orderAmount: 2 }] };
        const updatedOrder: Order = { ...getDefaultOrder(), id: 1, orderAmount: 3 };
        const action = managerUpdateOrderSuccess({ order: updatedOrder });

        const state = managerOrderReducer(initial, action);

        expect(state.orders[0].orderAmount).toBe(3);
    });

    it('should cancel an order on managerCancelOrderSuccess', () => {
        const initial = { ...initialState, orders: [{ ...getDefaultOrder(), id: 1, orderStatus: OrderStatus.InProcessing }] };
        const action = managerCancelOrderSuccess({ id: 1 });

        const state = managerOrderReducer(initial, action);

        expect(state.orders[0].orderStatus).toBe(OrderStatus.Canceled);
    });
});