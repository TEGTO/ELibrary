import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { BaseApiService, GetShopStatisticsRequest, mapShopStatisticsResponseToShopStatistics, ShopStatistics, ShopStatisticsResponse } from '../../../..';

@Injectable({
  providedIn: 'root'
})
export class StatisticsApiService extends BaseApiService {
  getShopStatistics(request: GetShopStatisticsRequest): Observable<ShopStatistics> {
    return this.httpClient.post<ShopStatisticsResponse>(this.combinePathWithStatisticsApiUrl(``), request).pipe(
      map((response) => mapShopStatisticsResponseToShopStatistics(response)),
      catchError((error) => this.handleError(error)),
    );
  }

  private combinePathWithStatisticsApiUrl(subpath: string): string {
    return this.urlDefiner.combineWithShopApiUrl("/statistics" + subpath);
  }
}