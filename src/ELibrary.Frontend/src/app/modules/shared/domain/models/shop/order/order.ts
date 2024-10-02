import { Client, getDefaultClient, OrderBook } from "../../../..";

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
    client: Client,
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
        client: getDefaultClient(),
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
export function getOrderUpdateStatuses(): { value: OrderStatus, name: string }[] {
    return [
        { value: OrderStatus.InProcessing, name: 'In Processing' },
        { value: OrderStatus.Processed, name: 'Processed' },
        { value: OrderStatus.Packed, name: 'Packed' },
    ]
}
export function getOrderPaymentMethodString(payment: PaymentMethod): string {
    switch (payment) {
        case PaymentMethod.Cash:
            return 'Cash';
        default:
            return "";
    }
}
export function getOrderCreateMinDate(): Date {
    const nextDay = new Date();
    nextDay.setDate(nextDay.getDate() + 1);
    nextDay.setHours(0, 0, 0, 0);
    return nextDay;
}
export function getCreatedOrderMinDate(order: Order): Date {
    const nextDay = new Date();
    nextDay.setDate(nextDay.getDate() + 1);
    nextDay.setHours(0, 0, 0, 0);
    return order.deliveryTime > nextDay ? nextDay : order.deliveryTime;
}