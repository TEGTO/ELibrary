import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { createBookstockOrder, getBookstockOrderAmount, getBookstockOrders, selectBookstockOrders, selectBookstockOrderTotalAmount } from '../..';
import { CreateStockBookOrderRequest, PaginatedRequest, StockBookOrder } from '../../../shared';
import { BookstockOrderService } from './bookstock-order-service';

@Injectable({
  providedIn: 'root'
})
export class BookstockOrderControllerService implements BookstockOrderService {

  constructor(
    private readonly store: Store,
  ) { }

  getPaginatedOrders(req: PaginatedRequest): Observable<StockBookOrder[]> {
    this.store.dispatch(getBookstockOrders({ req: req }));
    return this.store.select(selectBookstockOrders);
  }
  getOrderTotalAmount(): Observable<number> {
    this.store.dispatch(getBookstockOrderAmount());
    return this.store.select(selectBookstockOrderTotalAmount);
  }
  createOrder(req: CreateStockBookOrderRequest): void {
    this.store.dispatch(createBookstockOrder({ req: req }));
  }
}
