import { TestBed } from '@angular/core/testing';

import { CartControllerService } from './cart-controller.service';

describe('CartControllerService', () => {
  let service: CartControllerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CartControllerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
