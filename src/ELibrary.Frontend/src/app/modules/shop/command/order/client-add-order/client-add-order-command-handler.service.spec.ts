import { TestBed } from '@angular/core/testing';

import { ClientAddOrderCommandHandlerService } from './client-add-order-command-handler.service';

describe('ClientAddOrderCommandHandlerService', () => {
  let service: ClientAddOrderCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ClientAddOrderCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
