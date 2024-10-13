import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { CreateStockBookOrderRequest, mapStockBookOrderResponseToStockBookOrder, PaginatedRequest, StockBookOrder, StockBookOrderResponse } from '../../../..';
import { BaseApiService } from '../../base-api/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class BookstockApiService extends BaseApiService {

  getStockOrderById(id: number): Observable<StockBookOrder> {
    return this.httpClient.get<StockBookOrderResponse>(this.combinePathWithBookstockApiUrl(`/${id}`)).pipe(
      map((response) => mapStockBookOrderResponseToStockBookOrder(response)),
      catchError((error) => this.handleError(error))
    );
  }
  getStockOrderAmount(): Observable<number> {
    return this.httpClient.get<number>(this.combinePathWithBookstockApiUrl(`/amount`)).pipe(
      catchError((error) => this.handleError(error)),
    );
  }
  getStockOrderPaginated(req: PaginatedRequest): Observable<StockBookOrder[]> {
    return this.httpClient.post<StockBookOrderResponse[]>(this.combinePathWithBookstockApiUrl(`/pagination`), req).pipe(
      map((response) => response.map(x => mapStockBookOrderResponseToStockBookOrder(x))),
      catchError((error) => this.handleError(error)),
    );
  }
  createStockBookOrder(request: CreateStockBookOrderRequest): Observable<StockBookOrder> {
    return this.httpClient.post<StockBookOrderResponse>(this.combinePathWithBookstockApiUrl(``), request).pipe(
      map((response) => mapStockBookOrderResponseToStockBookOrder(response)),
      catchError((error) => this.handleError(error)),
    );
  }

  private combinePathWithBookstockApiUrl(subpath: string): string {
    return this.urlDefiner.combineWithShopApiUrl("/stockbook" + subpath);
  }

}
