import { Book } from "../../library/book";

export interface StockBookChange {
    id: number,
    book: Book,
    changeAmount: number
}