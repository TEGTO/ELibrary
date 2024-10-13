import { TestBed } from '@angular/core/testing';

import { DeleteAuthorCommandHandlerService } from './delete-author-command-handler.service';

describe('DeleteAuthorCommandHandlerService', () => {
  let service: DeleteAuthorCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DeleteAuthorCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
