import { TestBed } from '@angular/core/testing';

import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { URLDefiner } from '../../../url-definer/url-definer.service';
import { CartApiService } from './cart-api.service';

describe('CartApiService', () => {
  let service: CartApiService;
  let httpTestingController: HttpTestingController;
  let mockUrlDefiner: jasmine.SpyObj<URLDefiner>;

  beforeEach(() => {
    mockUrlDefiner = jasmine.createSpyObj<URLDefiner>('URLDefiner', ['combineWithShopApiUrl']);
    mockUrlDefiner.combineWithShopApiUrl.and.callFake((subpath: string) => `/api${subpath}`);
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        CartApiService,
        { provide: URLDefiner, useValue: mockUrlDefiner }
      ]
    });
    service = TestBed.inject(CartApiService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});