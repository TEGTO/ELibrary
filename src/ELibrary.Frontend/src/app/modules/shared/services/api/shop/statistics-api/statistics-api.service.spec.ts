/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { GetShopStatisticsRequest, mapShopStatisticsResponseToShopStatistics, ShopStatistics, ShopStatisticsResponse, URLDefiner } from '../../../..';
import { StatisticsApiService } from './statistics-api.service';

describe('StatisticsApiService', () => {
  let service: StatisticsApiService;
  let httpMock: HttpTestingController;
  let urlDefinerSpy: jasmine.SpyObj<URLDefiner>;

  const mockStatisticsResponse: ShopStatisticsResponse = {
    inCartCopies: 5,
    inOrderCopies: 10,
    soldCopies: 15,
    canceledCopies: 2,
    orderAmount: 2,
    canceledOrderAmount: 2,
    averagePrice: 20,
    earnedMoney: 300,
    orderAmountInDays: []
  };

  const mockStatisticsRequest: GetShopStatisticsRequest = {
    fromUTC: new Date(),
    toUTC: new Date(),
    includeBooks: []
  };

  const mockMappedStatistics: ShopStatistics = mapShopStatisticsResponseToShopStatistics(mockStatisticsResponse);

  beforeEach(() => {
    urlDefinerSpy = jasmine.createSpyObj('UrlDefiner', ['combineWithShopApiUrl']);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        StatisticsApiService,
        { provide: URLDefiner, useValue: urlDefinerSpy }
      ]
    });

    service = TestBed.inject(StatisticsApiService);
    httpMock = TestBed.inject(HttpTestingController);

    urlDefinerSpy.combineWithShopApiUrl.and.returnValue('/api/statistics');
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call the correct API endpoint and map the response', () => {
    service.getShopStatistics(mockStatisticsRequest).subscribe((statistics) => {
      expect(statistics).toEqual(mockMappedStatistics);
    });

    const req = httpMock.expectOne('/api/statistics');
    expect(req.request.method).toBe('POST');
    req.flush(mockStatisticsResponse);
  });

  it('should handle errors from the API', () => {
    const errorMessage = 'API error';

    spyOn(service as any, 'handleError').and.callThrough();

    service.getShopStatistics(mockStatisticsRequest).subscribe(
      () => fail('Expected error, but got success response'),
      () => {
        expect(service['handleError']).toHaveBeenCalled();
      }
    );

    const req = httpMock.expectOne('/api/statistics');
    req.flush(errorMessage, { status: 500, statusText: 'Server Error' });
  });

  it('should map the API response correctly', () => {
    const mappedStatistics = mapShopStatisticsResponseToShopStatistics(mockStatisticsResponse);
    expect(mappedStatistics).toEqual(mockMappedStatistics);
  });
});