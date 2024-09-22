import { Book } from "../../../..";

export interface AddBookToCartRequest {
    bookAmount: number;
    bookId: number;
}

export function mapBookToAddBookToCartRequest(book: Book): AddBookToCartRequest {
    return {
        bookAmount: 1,
        bookId: book.id
    }
}