import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { CreateStockBookOrderRequest, PaginatedRequest, StockBookOrder } from "../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class BookstockOrderService {
    abstract getPaginatedOrders(req: PaginatedRequest): Observable<StockBookOrder[]>;
    abstract getOrderTotalAmount(): Observable<number>;
    abstract createOrder(req: CreateStockBookOrderRequest): void;
}