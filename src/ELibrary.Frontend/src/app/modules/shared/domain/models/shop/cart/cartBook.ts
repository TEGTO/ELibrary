import { Book } from "../../../..";

export interface CartBook {
    id: string;
    bookAmount: number;
    bookId: number;
    book: Book;
}