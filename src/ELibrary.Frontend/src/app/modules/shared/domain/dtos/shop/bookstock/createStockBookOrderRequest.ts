import { StockBookChangeRequest } from "../../../..";

export interface CreateStockBookOrderRequest {
    clientId: string,
    stockBookChanges: StockBookChangeRequest[],
}