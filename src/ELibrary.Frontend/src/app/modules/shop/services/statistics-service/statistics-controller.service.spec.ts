import { TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { BookStatistics, GetBookStatistics, getDefaultBookStatistics, getDefaultGetBookStatistics, mapGetBookStatisticsToGetBookStatisticsRequest, StatisticsApiService } from '../../../shared';
import { StatisticsControllerService } from './statistics-controller.service';

describe('StatisticsControllerService', () => {
  let service: StatisticsControllerService;
  let apiServiceSpy: jasmine.SpyObj<StatisticsApiService>;

  const mockBookStatistics: BookStatistics = getDefaultBookStatistics();

  const mockGetBookStatistics: GetBookStatistics = getDefaultGetBookStatistics();

  beforeEach(() => {
    const apiServiceSpyObj = jasmine.createSpyObj('StatisticsApiService', ['getBookStatistics']);

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
    const mappedRequest = mapGetBookStatisticsToGetBookStatisticsRequest(mockGetBookStatistics);
    apiServiceSpy.getBookStatistics.and.returnValue(of(mockBookStatistics));

    service.getBookStatistics(mockGetBookStatistics).subscribe((result) => {
      expect(result).toEqual(mockBookStatistics);
    });

    expect(apiServiceSpy.getBookStatistics).toHaveBeenCalledWith(mappedRequest);
  });
});