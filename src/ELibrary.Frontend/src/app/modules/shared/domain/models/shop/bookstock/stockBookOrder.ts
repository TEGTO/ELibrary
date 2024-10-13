import { Client, getDefaultClient, getDefaultStockBookChange, StockBookChange } from "../../../..";

export enum StockBookOrderType {
    StockReplenishment, ClientOrder, ClientOrderCancel, ManagerOrderCancel
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

export function getDefaultStockBookOrder(): StockBookOrder {
    return {
        id: 0,
        createdAt: new Date(),
        updatedAt: new Date(),
        type: StockBookOrderType.ClientOrder,
        totalChangeAmount: 0,
        client: getDefaultClient(),
        stockBookChanges: [getDefaultStockBookChange()]
    }
}

export function stockBookOrderTypeToString(type: StockBookOrderType): string {
    switch (type) {
        case StockBookOrderType.StockReplenishment:
            return "Stock Replenishment"
        case StockBookOrderType.ClientOrder:
            return "Client Order"
        case StockBookOrderType.ClientOrderCancel:
            return "Client Order Cancel"
        case StockBookOrderType.ManagerOrderCancel:
            return "Manager Order Cancel"
        default:
            return "";
    }
}