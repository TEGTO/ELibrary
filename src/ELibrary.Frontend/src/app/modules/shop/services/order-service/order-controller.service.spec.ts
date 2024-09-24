import { TestBed } from '@angular/core/testing';

import { OrderControllerService } from './order-controller.service';

describe('OrderControllerService', () => {
  let service: OrderControllerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OrderControllerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
