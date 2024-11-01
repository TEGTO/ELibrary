import { Book, CartBook, getDefaultBook } from "../../../..";

export interface OrderBook {
    id: string;
    bookAmount: number;
    bookPrice: number;
    bookId: number;
    book: Book;
}

export function getDefaultOrderBook(): OrderBook {
    return {
        id: "",
        bookAmount: 0,
        bookPrice: 0,
        bookId: 0,
        book: getDefaultBook()
    }
}
export function mapCartBookToOrderBook(cartBook: CartBook): OrderBook {
    return {
        id: cartBook.id,
        bookPrice: cartBook.book.price,
        bookAmount: cartBook.bookAmount,
        bookId: cartBook.bookId,
        book: cartBook.book
    };
}