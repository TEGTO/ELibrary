import { TestBed } from '@angular/core/testing';

import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { URLDefiner } from '../../../..';
import { PublisherApiService } from './publisher-api.service';

describe('PublisherApiService', () => {
  let service: PublisherApiService;
  let httpTestingController: HttpTestingController;
  let mockUrlDefiner: jasmine.SpyObj<URLDefiner>;

  beforeEach(() => {
    mockUrlDefiner = jasmine.createSpyObj<URLDefiner>('URLDefiner', ['combineWithLibraryApiUrl']);
    mockUrlDefiner.combineWithLibraryApiUrl.and.callFake((subpath: string) => `/api${subpath}`);
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        PublisherApiService,
        { provide: URLDefiner, useValue: mockUrlDefiner }
      ]
    });
    service = TestBed.inject(PublisherApiService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});