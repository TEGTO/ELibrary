import { TestBed } from '@angular/core/testing';

import { ManagerUpdateOrderCommandHandlerService } from './manager-update-order-command-handler.service';

describe('ManagerUpdateOrderCommandHandlerService', () => {
  let service: ManagerUpdateOrderCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ManagerUpdateOrderCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
