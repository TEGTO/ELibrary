import { TestBed } from '@angular/core/testing';

import { BookstockApiService } from './bookstock-api.service';

describe('BookstockApiService', () => {
  let service: BookstockApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BookstockApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
