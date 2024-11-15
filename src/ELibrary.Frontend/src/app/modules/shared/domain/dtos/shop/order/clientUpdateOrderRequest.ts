import { DeliveryMethod, Order, PaymentMethod } from "../../../..";

export interface ClientUpdateOrderRequest {
    id: number,
    contactClientName: string,
    contactPhone: string,
    deliveryAddress: string,
    deliveryTime: Date,
    paymentMethod: PaymentMethod,
    deliveryMethod: DeliveryMethod,
}

export function mapOrderToClientUpdateOrderRequest(order: Order): ClientUpdateOrderRequest {
    return {
        id: order.id,
        contactClientName: order.contactClientName,
        contactPhone: order.contactPhone,
        deliveryAddress: order.deliveryAddress,
        deliveryTime: order.deliveryTime,
        paymentMethod: order.paymentMethod,
        deliveryMethod: order.deliveryMethod,
    }
}