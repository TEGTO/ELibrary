import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { CreateStockBookOrderRequest, PaginatedRequest, StockBookOrder } from "../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class BookstockOrderService {
    abstract getById(id: number): Observable<StockBookOrder>;
    abstract getPaginatedOrders(req: PaginatedRequest): Observable<StockBookOrder[]>;
    abstract getOrderTotalAmount(): Observable<number>;
    abstract createStockOrder(req: CreateStockBookOrderRequest): void;
}