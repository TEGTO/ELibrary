import { TestBed } from '@angular/core/testing';

import { CurrencyPipeApplierService } from './currency-pipe-applier.service';

describe('CurrencyPipeApplierService', () => {
  let service: CurrencyPipeApplierService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CurrencyPipeApplierService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
