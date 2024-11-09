import { Book, getDefaultBook } from "../../library/book";

export interface StockBookChange {
    id: number,
    bookId: number,
    book: Book,
    changeAmount: number
}

export function getDefaultStockBookChange(): StockBookChange {
    return {
        id: 0,
        bookId: 0,
        book: getDefaultBook(),
        changeAmount: 0
    }
}