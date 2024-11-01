import { BookResponse, mapBookResponseToBook, OrderBook } from "../../../..";

export interface OrderBookResponse {
    id: string;
    bookAmount: number;
    bookId: number;
    book: BookResponse;
    bookPrice: number;
}

export function mapOrderBookResponseToOrderBook(response: OrderBookResponse): OrderBook {
    return {
        id: response?.id,
        bookAmount: response?.bookAmount,
        bookPrice: response?.bookPrice,
        bookId: response?.bookId,
        book: mapBookResponseToBook(response?.book)
    }
}