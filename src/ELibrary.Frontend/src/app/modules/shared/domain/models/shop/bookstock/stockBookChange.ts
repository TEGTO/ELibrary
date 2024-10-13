import { Book, getDefaultBook } from "../../library/book";

export interface StockBookChange {
    id: number,
    book: Book,
    changeAmount: number
}

export function getDefaultStockBookChange(): StockBookChange {
    return {
        id: 0,
        book: getDefaultBook(),
        changeAmount: 0
    }
}