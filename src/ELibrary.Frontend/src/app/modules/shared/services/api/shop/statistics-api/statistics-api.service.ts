import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { BaseApiService, BookStatistics, BookStatisticsResponse, GetBookStatisticsRequest, mapBookStatisticsResponseToBookStatistics } from '../../../..';

@Injectable({
  providedIn: 'root'
})
export class StatisticsApiService extends BaseApiService {
  getBookStatistics(request: GetBookStatisticsRequest): Observable<BookStatistics> {
    return this.httpClient.post<BookStatisticsResponse>(this.combinePathWithStatisticsApiUrl(``), request).pipe(
      map((response) => mapBookStatisticsResponseToBookStatistics(response)),
      catchError((error) => this.handleError(error)),
    );
  }

  private combinePathWithStatisticsApiUrl(subpath: string): string {
    return this.urlDefiner.combineWithShopApiUrl("/statistics" + subpath);
  }
}