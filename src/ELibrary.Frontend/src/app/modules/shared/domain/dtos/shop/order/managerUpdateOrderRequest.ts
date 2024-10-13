import { Order, OrderStatus } from "../../../.."

export interface ManagerUpdateOrderRequest {
    id: number,
    deliveryAddress: string,
    deliveryTime: Date,
    orderStatus: OrderStatus
}

export function mapOrderToManagerUpdateOrderRequest(order: Order): ManagerUpdateOrderRequest {
    return {
        id: order.id,
        deliveryAddress: order.deliveryAddress,
        deliveryTime: order.deliveryTime,
        orderStatus: order.orderStatus
    }
}