import { TestBed } from '@angular/core/testing';

import { ClientUpdateOrderCommandHandlerService } from './client-update-order-command-handler.service';

describe('ClientUpdateOrderCommandHandlerService', () => {
  let service: ClientUpdateOrderCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ClientUpdateOrderCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
