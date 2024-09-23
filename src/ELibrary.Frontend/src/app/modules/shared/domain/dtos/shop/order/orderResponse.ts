import { BookListingResponse, mapBookListingResponseToOrderBook, Order, OrderStatus } from "../../../..";

export interface OrderResponse {
    id: number,
    creationTime: Date,
    orderAmount: number,
    totalPrice: number,
    deliveryAddress: string,
    deliveryTime: Date,
    orderStatus: OrderStatus,
    orderBooks: BookListingResponse[]
}

export function mapOrderResponseToOrder(response: OrderResponse): Order {
    return {
        id: response.id,
        creationTime: response.creationTime,
        orderAmount: response.orderAmount,
        totalPrice: response.totalPrice,
        deliveryAddress: response.deliveryAddress,
        deliveryTime: response.deliveryTime,
        orderStatus: response.orderStatus,
        orderBooks: response.orderBooks.map(x => mapBookListingResponseToOrderBook(x))
    }
}