import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BookStatistics, GetBookStatistics, mapGetBookStatisticsToGetBookStatisticsRequest, StatisticsApiService } from '../../../shared';
import { StatisticsService } from './statistics-service';

@Injectable({
  providedIn: 'root'
})
export class StatisticsControllerService implements StatisticsService {

  constructor(private readonly apiService: StatisticsApiService) { }

  getBookStatistics(getStats: GetBookStatistics): Observable<BookStatistics> {
    const request = mapGetBookStatisticsToGetBookStatisticsRequest(getStats);
    return this.apiService.getBookStatistics(request);
  }
}
