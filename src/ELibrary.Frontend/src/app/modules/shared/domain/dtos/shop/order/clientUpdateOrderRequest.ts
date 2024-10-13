import { DeliveryMethod, Order, PaymentMethod } from "../../../..";

export interface ClientUpdateOrderRequest {
    id: number,
    deliveryAddress: string,
    deliveryTime: Date,
    paymentMethod: PaymentMethod,
    deliveryMethod: DeliveryMethod,
}

export function mapOrderToClientUpdateOrderRequest(order: Order): ClientUpdateOrderRequest {
    return {
        id: order.id,
        deliveryAddress: order.deliveryAddress,
        deliveryTime: order.deliveryTime,
        paymentMethod: order.paymentMethod,
        deliveryMethod: order.deliveryMethod,
    }
}