import { TestBed } from '@angular/core/testing';

import { ManagerCancelOrderCommandHandlerService } from './manager-cancel-order-command-handler.service';

describe('ManagerCancelOrderCommandHandlerService', () => {
  let service: ManagerCancelOrderCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ManagerCancelOrderCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
