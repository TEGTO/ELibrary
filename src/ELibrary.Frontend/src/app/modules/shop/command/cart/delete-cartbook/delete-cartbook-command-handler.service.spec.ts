import { TestBed } from '@angular/core/testing';

import { DeleteCartBookCommandHandlerService } from './delete-cartbook-command-handler.service';

describe('DeleteCartbookCommandHandlerService', () => {
  let service: DeleteCartBookCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DeleteCartBookCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
