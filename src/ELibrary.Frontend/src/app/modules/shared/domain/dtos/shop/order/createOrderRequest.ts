import { DeliveryMethod, mapOrderBookToOrderBookRequest, Order, OrderBookRequest, PaymentMethod } from "../../../..";

export interface CreateOrderRequest {
    contactClientName: string,
    contactPhone: string,
    deliveryAddress: string,
    deliveryTime: Date,
    paymentMethod: PaymentMethod,
    deliveryMethod: DeliveryMethod,
    orderBooks: OrderBookRequest[]
}

export function mapOrderToCreateOrderRequest(order: Order): CreateOrderRequest {
    return {
        contactClientName: order.contactClientName,
        contactPhone: order.contactPhone,
        deliveryAddress: order.deliveryAddress,
        deliveryTime: order.deliveryTime,
        paymentMethod: order.paymentMethod,
        deliveryMethod: order.deliveryMethod,
        orderBooks: order.orderBooks.map(x => mapOrderBookToOrderBookRequest(x))
    }
}

export function getDefaultCreateOrderRequest(): CreateOrderRequest {
    return {
        contactClientName: "",
        contactPhone: "",
        deliveryAddress: "",
        deliveryTime: new Date(),
        paymentMethod: PaymentMethod.Cash,
        deliveryMethod: DeliveryMethod.AddressDelivery,
        orderBooks: []
    }
}