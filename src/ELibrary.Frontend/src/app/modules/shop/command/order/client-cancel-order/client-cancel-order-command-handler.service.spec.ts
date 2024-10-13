import { TestBed } from '@angular/core/testing';

import { ClientCancelOrderCommandHandlerService } from './client-cancel-order-command-handler.service';

describe('ClientCancelOrderCommandHandlerService', () => {
  let service: ClientCancelOrderCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ClientCancelOrderCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
