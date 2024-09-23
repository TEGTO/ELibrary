import { Book } from "../../../..";

export interface OrderBook {
    id: string;
    bookAmount: number;
    bookId: number;
    book: Book;
}