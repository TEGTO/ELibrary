import { Book, CartBook } from "../../../..";

export interface OrderBook {
    id: string;
    bookAmount: number;
    bookId: number;
    book: Book;
}

export function mapCartBookToOrderBook(cartBook: CartBook): OrderBook {
    return {
        id: cartBook.id,
        bookAmount: cartBook.bookAmount,
        bookId: cartBook.bookId,
        book: cartBook.book
    };
}