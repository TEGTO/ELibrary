import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ClientUpdateOrderRequest, CreateOrderRequest, GetOrdersFilter, ManagerUpdateOrderRequest, Order } from "../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class OrderService {
    abstract managerGetOrderById(id: number): Observable<Order>;
    abstract getPaginatedOrders(req: GetOrdersFilter): Observable<Order[]>;
    abstract getOrderTotalAmount(req: GetOrdersFilter): Observable<number>;
    abstract createOrder(req: CreateOrderRequest): Observable<{ isSuccess: boolean, error: unknown }>;
    abstract clientUpdateOrder(req: ClientUpdateOrderRequest): void;
    abstract clientCancelOrder(id: number): void;
    abstract managerGetPaginatedOrders(req: GetOrdersFilter): Observable<Order[]>;
    abstract managerGetOrderAmount(req: GetOrdersFilter): Observable<number>;
    abstract managerUpdateOrder(req: ManagerUpdateOrderRequest): void;
    abstract managerCancelOrder(id: number): void;
}