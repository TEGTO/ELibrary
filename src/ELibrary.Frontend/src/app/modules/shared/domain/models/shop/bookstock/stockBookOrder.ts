import { Client, StockBookChange } from "../../../..";

export interface StockBookOrder {
    id: number,
    createdAt: Date,
    updatedAt: Date,
    totalChangeAmount: number,
    client: Client,
    stockBookChanges: StockBookChange[]
}