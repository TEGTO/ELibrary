import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GetShopStatistics, mapGetShopStatisticsToGetShopStatisticsRequest, ShopStatistics, StatisticsApiService } from '../../../shared';
import { StatisticsService } from './statistics-service';

@Injectable({
  providedIn: 'root'
})
export class StatisticsControllerService implements StatisticsService {

  constructor(private readonly apiService: StatisticsApiService) { }

  getShopStatistics(getStats: GetShopStatistics): Observable<ShopStatistics> {
    const request = mapGetShopStatisticsToGetShopStatisticsRequest(getStats);
    return this.apiService.getShopStatistics(request);
  }
}
