import { OrderBook } from "../../../..";

export interface OrderBookRequest {
    bookAmount: number,
    bookId: number
}
export function mapOrderBookToOrderBookRequest(orderBook: OrderBook): OrderBookRequest {
    return {
        bookAmount: orderBook.bookAmount,
        bookId: orderBook.bookId
    }
}