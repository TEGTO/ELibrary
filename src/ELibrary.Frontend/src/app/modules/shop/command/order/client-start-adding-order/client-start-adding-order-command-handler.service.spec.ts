import { TestBed } from '@angular/core/testing';

import { ClientStartAddingOrderCommandHandlerService } from './client-start-adding-order-command-handler.service';

describe('ClientStartAddingOrderCommandHandlerService', () => {
  let service: ClientStartAddingOrderCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ClientStartAddingOrderCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
