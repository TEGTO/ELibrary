import { Book, getDefaultBook } from "../../../..";

export interface CartBook {
    id: string;
    bookAmount: number;
    bookId: number;
    book: Book;
}

export function getDefaultCartBook(): CartBook {
    return {
        id: "",
        bookAmount: 0,
        bookId: 0,
        book: getDefaultBook()
    }
}