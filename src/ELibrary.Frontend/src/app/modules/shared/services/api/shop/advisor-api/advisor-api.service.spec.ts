
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { AdvisorQueryRequest, AdvisorResponse, URLDefiner } from '../../../..';
import { AdvisorApiService } from './advisor-api.service';

describe('AdvisorApiService', () => {
  let service: AdvisorApiService;
  let httpMock: HttpTestingController;
  let mockUrlDefiner: jasmine.SpyObj<URLDefiner>;

  beforeEach(() => {
    mockUrlDefiner = jasmine.createSpyObj('UrlDefinerService', ['combineWithShopApiUrl']);

    TestBed.configureTestingModule({
      providers: [
        AdvisorApiService,
        {
          provide: URLDefiner, useValue: mockUrlDefiner,
        },
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });

    service = TestBed.inject(AdvisorApiService);
    httpMock = TestBed.inject(HttpTestingController);

    mockUrlDefiner.combineWithShopApiUrl.and.returnValue('https://api.example.com/advisor');
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should send query and return the expected response', () => {
    const mockRequest: AdvisorQueryRequest = { query: 'test query' };
    const mockResponse: AdvisorResponse = { message: 'test response' };
    service.sendQuery(mockRequest).subscribe((response) => {
      expect(response).toEqual(mockResponse);
    });

    const req = httpMock.expectOne('https://api.example.com/advisor');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(mockRequest);

    req.flush(mockResponse);
  });
});