import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ClientUpdateOrderRequest, CreateOrderRequest, ManagerUpdateOrderRequest, Order, PaginatedRequest } from "../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class OrderService {
    abstract managerGetOrderById(id: number): Observable<Order>;
    abstract getPaginatedOrders(req: PaginatedRequest): Observable<Order[]>;
    abstract getOrderTotalAmount(): Observable<number>;
    abstract createOrder(req: CreateOrderRequest): Observable<{ isSuccess: boolean, error: unknown }>;
    abstract clientUpdateOrder(req: ClientUpdateOrderRequest): void;
    abstract clientCancelOrder(id: number): void;
    abstract managerGetPaginatedOrders(req: PaginatedRequest): Observable<Order[]>;
    abstract managerGetOrderAmount(): Observable<number>;
    abstract managerUpdateOrder(req: ManagerUpdateOrderRequest): void;
    abstract managerCancelOrder(id: number): void;
}