import { TestBed } from '@angular/core/testing';

import { StatisticsControllerService } from './statistics-controller.service';

describe('StatisticsControllerService', () => {
  let service: StatisticsControllerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StatisticsControllerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
