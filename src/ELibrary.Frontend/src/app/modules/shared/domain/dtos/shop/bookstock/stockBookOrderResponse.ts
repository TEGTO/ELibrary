import { ClientResponse, mapClientResponseToClient, mapStockBookChangeResponseToStockBookChange, StockBookChangeResponse, StockBookOrder, StockBookOrderType } from "../../../..";

export interface StockBookOrderResponse {
    id: number,
    createdAt: Date,
    updatedAt: Date,
    type: StockBookOrderType,
    totalChangeAmount: number,
    client: ClientResponse,
    stockBookChanges: StockBookChangeResponse[]
}

export function mapStockBookOrderResponseToStockBookOrder(response: StockBookOrderResponse): StockBookOrder {
    return {
        id: response.id,
        createdAt: response.createdAt,
        updatedAt: response.updatedAt,
        type: response.type,
        totalChangeAmount: response.totalChangeAmount,
        client: mapClientResponseToClient(response.client),
        stockBookChanges: response.stockBookChanges.map(x => mapStockBookChangeResponseToStockBookChange(x)),
    }
}