import { StockBookChange } from "../../../.."

export interface StockBookChangeRequest {
    bookId: number,
    changeAmount: number
}

export function mapStockBookChangeToStockBookChangeRequest(stockBookChange: StockBookChange): StockBookChangeRequest {
    return {
        bookId: stockBookChange.book.id,
        changeAmount: stockBookChange.changeAmount
    }
}