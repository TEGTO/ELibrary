import { BookResponse, CartBook, mapBookResponseToBook } from "../../../..";

export interface CartBookResponse {
    id: string;
    bookAmount: number;
    bookId: number;
    book: BookResponse;
}

export function mapCartBookResponseToCartBook(response: CartBookResponse): CartBook {
    return {
        id: response?.id,
        bookAmount: response?.bookAmount,
        bookId: response?.bookId,
        book: mapBookResponseToBook(response?.book)
    }
}