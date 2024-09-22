import { TestBed } from '@angular/core/testing';

import { ShopDialogManagerService } from './shop-dialog-manager.service';

describe('ShopDialogManagerService', () => {
  let service: ShopDialogManagerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ShopDialogManagerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
