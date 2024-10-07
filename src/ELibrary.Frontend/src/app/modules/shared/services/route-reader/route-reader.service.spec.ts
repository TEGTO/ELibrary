import { TestBed } from '@angular/core/testing';

import { RouteReaderService } from './route-reader.service';

describe('RouteReaderService', () => {
  let service: RouteReaderService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RouteReaderService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
