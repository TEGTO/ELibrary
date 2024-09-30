import { TestBed } from '@angular/core/testing';

import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { URLDefiner } from '../../../url-definer/url-definer.service';
import { OrderApiService } from './order-api.service';

describe('OrderApiService', () => {
  let service: OrderApiService;
  let httpTestingController: HttpTestingController;
  let mockUrlDefiner: jasmine.SpyObj<URLDefiner>;

  beforeEach(() => {
    mockUrlDefiner = jasmine.createSpyObj<URLDefiner>('URLDefiner', ['combineWithShopApiUrl']);
    mockUrlDefiner.combineWithShopApiUrl.and.callFake((subpath: string) => `/api${subpath}`);
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        OrderApiService,
        { provide: URLDefiner, useValue: mockUrlDefiner }
      ]
    });
    service = TestBed.inject(OrderApiService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});