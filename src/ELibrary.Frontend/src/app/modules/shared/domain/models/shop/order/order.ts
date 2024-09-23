import { OrderBook } from "../../../..";

export enum OrderStatus {
    InProcessing, Packed, Delivered
}
export interface Order {
    id: number,
    creationTime: Date,
    orderAmount: number,
    totalPrice: number,
    deliveryAddress: string,
    deliveryTime: Date,
    orderStatus: OrderStatus,
    orderBooks: OrderBook[]
}