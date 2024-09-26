import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { BaseApiService, ClientUpdateOrderRequest, CreateOrderRequest, ManagerUpdateOrderRequest, mapOrderResponseToOrder, Order, OrderResponse, PaginatedRequest } from '../../../..';

@Injectable({
  providedIn: 'root'
})
export class OrderApiService extends BaseApiService {
  getPaginatedOrders(request: PaginatedRequest): Observable<Order[]> {
    return this.httpClient.post<OrderResponse[]>(this.combinePathWithOrderApiUrl(`/pagination`), request).pipe(
      map((response) => response.map(x => mapOrderResponseToOrder(x))),
      catchError((error) => this.handleError(error)),
    );
  }
  getOrderAmount(): Observable<number> {
    return this.httpClient.get<number>(this.combinePathWithOrderApiUrl(`/amount`)).pipe(
      catchError((error) => this.handleError(error)),
    );
  }
  createOrder(request: CreateOrderRequest): Observable<Order> {
    return this.httpClient.post<OrderResponse>(this.combinePathWithOrderApiUrl(``), request).pipe(
      map((response) => mapOrderResponseToOrder(response)),
      catchError((error) => this.handleError(error)),
    );
  }
  updateOrder(request: ClientUpdateOrderRequest): Observable<Order> {
    return this.httpClient.patch<OrderResponse>(this.combinePathWithOrderApiUrl(``), request).pipe(
      map((response) => mapOrderResponseToOrder(response)),
      catchError((error) => this.handleError(error)),
    );
  }
  cancelOrder(id: number): Observable<HttpResponse<void>> {
    return this.httpClient.delete<void>(this.combinePathWithOrderApiUrl(`/${id}`), { observe: 'response' }).pipe(
      catchError((error) => this.handleError(error))
    );
  }
  managerGetPaginatedOrders(request: PaginatedRequest): Observable<Order[]> {
    return this.httpClient.post<OrderResponse[]>(this.combinePathWithOrderApiUrl(`/manager/pagination`), request).pipe(
      map((response) => response.map(x => mapOrderResponseToOrder(x))),
      catchError((error) => this.handleError(error)),
    );
  }
  managerGetOrderAmount(): Observable<number> {
    return this.httpClient.get<number>(this.combinePathWithOrderApiUrl(`/manager/amount`)).pipe(
      catchError((error) => this.handleError(error)),
    );
  }
  managerUpdateOrder(request: ManagerUpdateOrderRequest): Observable<Order> {
    return this.httpClient.put<OrderResponse>(this.combinePathWithOrderApiUrl(`/manager`), request).pipe(
      map((response) => mapOrderResponseToOrder(response)),
      catchError((error) => this.handleError(error)),
    );
  }
  managerCancelOrder(id: number): Observable<HttpResponse<void>> {
    return this.httpClient.delete<void>(this.combinePathWithOrderApiUrl(`/manager/${id}`), { observe: 'response' }).pipe(
      catchError((error) => this.handleError(error))
    );
  }

  private combinePathWithOrderApiUrl(subpath: string): string {
    return this.urlDefiner.combineWithShopApiUrl("/order" + subpath);
  }
}

