import { TestBed } from '@angular/core/testing';

import { PublisherControllerService } from './publisher-controller.service';

describe('PublisherControllerService', () => {
  let service: PublisherControllerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PublisherControllerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
