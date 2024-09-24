import { BookListingResponse, mapBookListingResponseToOrderBook, Order, OrderStatus, PaymentMethod } from "../../../..";

export interface OrderResponse {
    id: number,
    creationTime: Date,
    orderAmount: number,
    totalPrice: number,
    deliveryAddress: string,
    deliveryTime: Date,
    orderStatus: OrderStatus,
    paymentMethod: PaymentMethod,
    orderBooks: BookListingResponse[]
}

export function mapOrderResponseToOrder(response: OrderResponse): Order {
    return {
        id: response.id,
        creationTime: new Date(response.creationTime),
        orderAmount: response.orderAmount,
        totalPrice: response.totalPrice,
        deliveryAddress: response.deliveryAddress,
        deliveryTime: new Date(response.deliveryTime),
        orderStatus: response.orderStatus,
        paymentMethod: response.paymentMethod,
        orderBooks: response.orderBooks.map(x => mapBookListingResponseToOrderBook(x))
    }
}