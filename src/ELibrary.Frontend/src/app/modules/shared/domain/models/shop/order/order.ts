import { OrderBook } from "../../../..";

export enum OrderStatus {
    Canceled = -1, InProcessing, Processed, Packed, Delivered
}
export enum PaymentMethod {
    Cash
}
export interface Order {
    id: number,
    createdAt: Date,
    updatedAt: Date,
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
        createdAt: new Date(),
        updatedAt: new Date(),
        orderAmount: 0,
        totalPrice: 0,
        deliveryAddress: "",
        deliveryTime: new Date(),
        orderStatus: OrderStatus.InProcessing,
        paymentMethod: PaymentMethod.Cash,
        orderBooks: []
    };
}

export function getOrderStatusString(orderStatus: OrderStatus): string {
    switch (orderStatus) {
        case OrderStatus.Canceled:
            return 'Canceled';
        case OrderStatus.InProcessing:
            return 'In Processing';
        case OrderStatus.Processed:
            return 'Processed';
        case OrderStatus.Packed:
            return 'Packed';
        case OrderStatus.Delivered:
            return 'Delivered';
        default:
            return "";
    }
}