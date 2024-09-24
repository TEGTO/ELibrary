import { OrderBook } from "../../../..";

export enum OrderStatus {
    Canceled = -1, InProcessing, Packed, Delivered
}
export enum PaymentMethod {
    Cash
}
export interface Order {
    id: number,
    creationTime: Date,
    orderAmount: number,
    totalPrice: number,
    deliveryAddress: string,
    deliveryTime: Date,
    orderStatus: OrderStatus,
    paymentMethod: PaymentMethod,
    orderBooks: OrderBook[]
}

export function getDefaultOrder(): Order {
    return {
        id: 0,
        creationTime: new Date(),
        orderAmount: 0,
        totalPrice: 0,
        deliveryAddress: "",
        deliveryTime: new Date(),
        orderStatus: OrderStatus.InProcessing,
        paymentMethod: PaymentMethod.Cash,
        orderBooks: []
    };
}