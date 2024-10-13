import { BookListingResponse, ClientResponse, DeliveryMethod, mapBookListingResponseToOrderBook, mapClientResponseToClient, Order, OrderStatus, PaymentMethod } from "../../../..";

export interface OrderResponse {
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
    client: ClientResponse,
    orderBooks: BookListingResponse[]
}

export function mapOrderResponseToOrder(response: OrderResponse): Order {
    return {
        id: response?.id,
        createdAt: new Date(response?.createdAt),
        updatedAt: new Date(response?.updatedAt),
        orderAmount: response?.orderAmount,
        totalPrice: response?.totalPrice,
        deliveryAddress: response?.deliveryAddress,
        deliveryTime: new Date(response?.deliveryTime),
        orderStatus: response?.orderStatus,
        paymentMethod: response?.paymentMethod,
        deliveryMethod: response?.deliveryMethod,
        client: mapClientResponseToClient(response?.client),
        orderBooks: response?.orderBooks.map(x => mapBookListingResponseToOrderBook(x))
    }
}