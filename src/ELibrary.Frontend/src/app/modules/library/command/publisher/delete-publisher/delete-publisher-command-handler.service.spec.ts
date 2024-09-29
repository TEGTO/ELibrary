import { TestBed } from '@angular/core/testing';

import { DeletePublisherCommandHandlerService } from './delete-publisher-command-handler.service';

describe('DeletePublisherCommandHandlerService', () => {
  let service: DeletePublisherCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DeletePublisherCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
