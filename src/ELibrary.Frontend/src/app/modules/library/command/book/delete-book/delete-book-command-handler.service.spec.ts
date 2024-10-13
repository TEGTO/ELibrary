import { TestBed } from '@angular/core/testing';

import { DeleteBookCommandHandlerService } from './delete-book-command-handler.service';

describe('DeleteBookCommandHandlerService', () => {
  let service: DeleteBookCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DeleteBookCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
