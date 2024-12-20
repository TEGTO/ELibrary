import { ClientResponse, DeliveryMethod, mapClientResponseToClient, mapOrderBookResponseToOrderBook, Order, OrderBookResponse, OrderStatus, PaymentMethod } from "../../../..";

export interface OrderResponse {
    id: number,
    createdAt: Date,
    updatedAt: Date,
    orderAmount: number,
    totalPrice: number,
    contactClientName: string,
    contactPhone: string,
    deliveryAddress: string,
    deliveryTime: Date,
    orderStatus: OrderStatus,
    paymentMethod: PaymentMethod,
    deliveryMethod: DeliveryMethod,
    client: ClientResponse,
    orderBooks: OrderBookResponse[]
}

export function mapOrderResponseToOrder(response: OrderResponse): Order {
    return {
        id: response?.id,
        createdAt: new Date(response?.createdAt),
        updatedAt: new Date(response?.updatedAt),
        orderAmount: response?.orderAmount,
        totalPrice: response?.totalPrice,
        contactClientName: response?.contactClientName,
        contactPhone: response?.contactPhone,
        deliveryAddress: response?.deliveryAddress,
        deliveryTime: new Date(response?.deliveryTime),
        orderStatus: response?.orderStatus,
        paymentMethod: response?.paymentMethod,
        deliveryMethod: response?.deliveryMethod,
        client: mapClientResponseToClient(response?.client),
        orderBooks: response?.orderBooks.map(x => mapOrderBookResponseToOrderBook(x))
    }
}