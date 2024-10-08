import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { combineLatest, map, Observable } from 'rxjs';
import { cancelOrder, createOrder, getOrders, getOrderTotalAmount, managerCancelOrder, managerGetOrderById, managerGetOrderTotalAmount, managerGetPaginatedOrders, managerUpdateOrder, selectIsCreateSuccess, selectManagerOrderById, selectManagerOrders, selectManagerOrderTotalAmount, selectOrderErrors, selectOrders, selectOrderTotalAmount, updateOrder } from '../..';
import { ClientUpdateOrderRequest, CreateOrderRequest, GetOrdersFilter, ManagerUpdateOrderRequest, Order } from '../../../shared';
import { OrderService } from './order-service';

@Injectable({
  providedIn: 'root'
})
export class OrderControllerService implements OrderService {

  constructor(
    private readonly store: Store,
  ) { }

  getPaginatedOrders(req: GetOrdersFilter): Observable<Order[]> {
    this.store.dispatch(getOrders({ req: req }));
    return this.store.select(selectOrders);
  }
  getOrderTotalAmount(req: GetOrdersFilter): Observable<number> {
    this.store.dispatch(getOrderTotalAmount({ req: req }));
    return this.store.select(selectOrderTotalAmount);
  }
  createOrder(req: CreateOrderRequest): Observable<{ isSuccess: boolean, error: unknown }> {
    this.store.dispatch(createOrder({ req }));

    return combineLatest([
      this.store.select(selectIsCreateSuccess),
      this.store.select(selectOrderErrors)
    ]).pipe(
      map(([isSuccess, error]) => ({ isSuccess: isSuccess, error: error }))
    );
  }
  clientUpdateOrder(req: ClientUpdateOrderRequest): void {
    this.store.dispatch(updateOrder({ req: req }));
  }
  clientCancelOrder(id: number): void {
    this.store.dispatch(cancelOrder({ id: id }));
  }
  managerGetOrderById(id: number): Observable<Order> {
    this.store.dispatch(managerGetOrderById({ id: id }));
    return this.store.select(selectManagerOrderById(id));
  }
  managerGetPaginatedOrders(req: GetOrdersFilter): Observable<Order[]> {
    this.store.dispatch(managerGetPaginatedOrders({ req: req }));
    return this.store.select(selectManagerOrders);
  }
  managerGetOrderAmount(req: GetOrdersFilter): Observable<number> {
    this.store.dispatch(managerGetOrderTotalAmount({ req: req }));
    return this.store.select(selectManagerOrderTotalAmount);
  }
  managerUpdateOrder(req: ManagerUpdateOrderRequest): void {
    this.store.dispatch(managerUpdateOrder({ req: req }));
  }
  managerCancelOrder(id: number): void {
    this.store.dispatch(managerCancelOrder({ id: id }));
  }
}
