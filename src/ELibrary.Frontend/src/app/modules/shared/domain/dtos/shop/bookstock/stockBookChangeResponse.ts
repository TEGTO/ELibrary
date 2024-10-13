import { BookResponse, mapBookResponseToBook, StockBookChange } from "../../../..";

export interface StockBookChangeResponse {
    id: number,
    book: BookResponse,
    changeAmount: number
}

export function mapStockBookChangeResponseToStockBookChange(response: StockBookChangeResponse): StockBookChange {
    return {
        id: response.id,
        book: mapBookResponseToBook(response.book),
        changeAmount: response.changeAmount,
    }
}