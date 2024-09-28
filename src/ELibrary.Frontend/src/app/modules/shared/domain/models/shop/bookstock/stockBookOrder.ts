import { Client, StockBookChange } from "../../../..";

export enum StockBookOrderType {
    StockReplenishment, ClientOrder, ClientOrderCancel
}
export interface StockBookOrder {
    id: number,
    createdAt: Date,
    updatedAt: Date,
    type: StockBookOrderType,
    totalChangeAmount: number,
    client: Client,
    stockBookChanges: StockBookChange[]
}

export function stockBookOrderTypeToString(type: StockBookOrderType): string {
    switch (type) {
        case StockBookOrderType.StockReplenishment:
            return "Stock Replenishment"
        case StockBookOrderType.ClientOrder:
            return "Client Order"
        case StockBookOrderType.ClientOrderCancel:
            return "Client Order Cancel"
        default:
            return "";
    }
}