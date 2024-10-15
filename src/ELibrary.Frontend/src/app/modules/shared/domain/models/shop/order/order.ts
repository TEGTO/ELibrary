import { Client, getDefaultClient, OrderBook } from "../../../..";
import { environment } from "../../../../../../../environment/environment";

export enum OrderStatus {
    Canceled = -1, InProcessing, Completed
}
export enum PaymentMethod {
    Cash
}
export enum DeliveryMethod {
    SelfPickup, AddressDelivery
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
    deliveryMethod: DeliveryMethod,
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
        deliveryMethod: DeliveryMethod.SelfPickup,
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
        case OrderStatus.Completed:
            return 'Completed';
        default:
            return "";
    }
}
export function getOrderUpdateStatuses(): { value: OrderStatus, name: string }[] {
    return [
        { value: OrderStatus.InProcessing, name: getOrderStatusString(OrderStatus.InProcessing) },
        { value: OrderStatus.Completed, name: getOrderStatusString(OrderStatus.Completed) },
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
export function getDeliveryMethodsString(delivery: DeliveryMethod): string {
    switch (delivery) {
        case DeliveryMethod.SelfPickup:
            return 'Self Pickup';
        case DeliveryMethod.AddressDelivery:
            return 'Address Delivery';
        default:
            return "";
    }
}

export function getOrderCreateMinDate(): Date {
    const nextDay = new Date();
    nextDay.setDate(nextDay.getDate() + 1);
    nextDay.setHours(environment.minOrderTime.getHours(), environment.minOrderTime.getMinutes());
    return nextDay;
}
export function getCreatedOrderMinDate(order: Order): Date {
    const minDate = getOrderCreateMinDate();
    return order.deliveryTime > minDate ? minDate : order.deliveryTime;
}

export function getOrderDeliveryMethods(): { value: DeliveryMethod, name: string }[] {
    return [
        { value: DeliveryMethod.SelfPickup, name: getDeliveryMethodsString(DeliveryMethod.SelfPickup) },
        { value: DeliveryMethod.AddressDelivery, name: getDeliveryMethodsString(DeliveryMethod.AddressDelivery) },
    ]
}
export function getPaymentMethods(): { value: PaymentMethod, name: string }[] {
    return [
        { value: PaymentMethod.Cash, name: getOrderPaymentMethodString(PaymentMethod.Cash) },
    ]
}