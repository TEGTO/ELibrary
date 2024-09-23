import { mapOrderBookToOrderBookRequest, Order, OrderBookRequest } from "../../../..";

export interface CreateOrderRequest {
    deliveryAddress: string,
    deliveryTime: Date,
    orderBooks: OrderBookRequest[]
}

export function mapOrderToCreateOrderRequest(order: Order): CreateOrderRequest {
    return {
        deliveryAddress: order.deliveryAddress,
        deliveryTime: order.deliveryTime,
        orderBooks: order.orderBooks.map(x => mapOrderBookToOrderBookRequest(x))
    }
}