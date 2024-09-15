import { TestBed } from '@angular/core/testing';

import { PublisherApiService } from './publisher-api.service';

describe('PublisherApiService', () => {
  let service: PublisherApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PublisherApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
