import { StockBookChangeRequest, StockBookOrderType } from "../../../..";

export interface CreateStockBookOrderRequest {
    type: StockBookOrderType,
    clientId: string,
    stockBookChanges: StockBookChangeRequest[],
}