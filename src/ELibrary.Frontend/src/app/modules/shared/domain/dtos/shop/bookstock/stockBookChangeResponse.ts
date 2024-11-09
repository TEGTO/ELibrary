import { BookResponse, mapBookResponseToBook, StockBookChange } from "../../../..";

export interface StockBookChangeResponse {
    id: number,
    bookId: number,
    book: BookResponse,
    changeAmount: number
}

export function mapStockBookChangeResponseToStockBookChange(response: StockBookChangeResponse): StockBookChange {
    return {
        id: response.id,
        bookId: response.bookId,
        book: mapBookResponseToBook(response.book),
        changeAmount: response.changeAmount,
    }
}