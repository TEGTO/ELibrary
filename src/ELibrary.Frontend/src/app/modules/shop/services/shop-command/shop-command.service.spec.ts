import { TestBed } from '@angular/core/testing';

import { ShopCommandService } from './shop-command.service';

describe('ShopCommandService', () => {
  let service: ShopCommandService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ShopCommandService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
