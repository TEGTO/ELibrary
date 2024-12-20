import { TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { getDefaultGetShopStatistics, getDefaultShopStatistics, GetShopStatistics, mapGetShopStatisticsToGetShopStatisticsRequest, ShopStatistics, StatisticsApiService } from '../../../shared';
import { StatisticsControllerService } from './statistics-controller.service';

describe('StatisticsControllerService', () => {
  let service: StatisticsControllerService;
  let apiServiceSpy: jasmine.SpyObj<StatisticsApiService>;

  const mockBookStatistics: ShopStatistics = getDefaultShopStatistics();

  const mockGetBookStatistics: GetShopStatistics = getDefaultGetShopStatistics();

  beforeEach(() => {
    const apiServiceSpyObj = jasmine.createSpyObj<StatisticsApiService>(['getShopStatistics']);

    TestBed.configureTestingModule({
      providers: [
        StatisticsControllerService,
        { provide: StatisticsApiService, useValue: apiServiceSpyObj }
      ]
    });

    service = TestBed.inject(StatisticsControllerService);
    apiServiceSpy = TestBed.inject(StatisticsApiService) as jasmine.SpyObj<StatisticsApiService>;
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call apiService.getBookStatistics with correct request and return expected statistics', () => {
    const mappedRequest = mapGetShopStatisticsToGetShopStatisticsRequest(mockGetBookStatistics);
    apiServiceSpy.getShopStatistics.and.returnValue(of(mockBookStatistics));

    service.getShopStatistics(mockGetBookStatistics).subscribe((result) => {
      expect(result).toEqual(mockBookStatistics);
    });

    expect(apiServiceSpy.getShopStatistics).toHaveBeenCalledWith(mappedRequest);
  });
});