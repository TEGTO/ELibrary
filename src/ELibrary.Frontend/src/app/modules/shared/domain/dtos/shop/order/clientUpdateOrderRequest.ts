import { Order } from "../../../..";

export interface ClientUpdateOrderRequest {
    id: number,
    deliveryAddress: string,
    deliveryTime: Date
}

export function mapOrderToClientUpdateOrderRequest(order: Order): ClientUpdateOrderRequest {
    return {
        id: order.id,
        deliveryAddress: order.deliveryAddress,
        deliveryTime: order.deliveryTime
    }
}