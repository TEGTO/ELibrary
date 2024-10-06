import { DeliveryMethod, mapOrderBookToOrderBookRequest, Order, OrderBookRequest, PaymentMethod } from "../../../..";

export interface CreateOrderRequest {
    deliveryAddress: string,
    deliveryTime: Date,
    paymentMethod: PaymentMethod,
    deliveryMethod: DeliveryMethod,
    orderBooks: OrderBookRequest[]
}

export function mapOrderToCreateOrderRequest(order: Order): CreateOrderRequest {
    return {
        deliveryAddress: order.deliveryAddress,
        deliveryTime: order.deliveryTime,
        paymentMethod: order.paymentMethod,
        deliveryMethod: order.deliveryMethod,
        orderBooks: order.orderBooks.map(x => mapOrderBookToOrderBookRequest(x))
    }
}