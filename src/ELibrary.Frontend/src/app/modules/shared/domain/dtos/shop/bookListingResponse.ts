import { BookResponse, CartBook, mapBookResponseToBook, OrderBook } from "../../..";

export interface BookListingResponse {
    id: string;
    bookAmount: number;
    bookId: number;
    book: BookResponse;
}

export function mapBookListingResponseToCartBook(response: BookListingResponse): CartBook {
    return {
        id: response?.id,
        bookAmount: response?.bookAmount,
        bookId: response?.bookId,
        book: mapBookResponseToBook(response?.book)
    }
}
export function mapBookListingResponseToOrderBook(response: BookListingResponse): OrderBook {
    return {
        id: response?.id,
        bookAmount: response?.bookAmount,
        bookId: response?.bookId,
        book: mapBookResponseToBook(response?.book)
    }
}