import { TestBed } from '@angular/core/testing';

import { CreateAuthorCommandHandlerService } from './create-author-command-handler.service';

describe('CreateAuthorCommandHandlerService', () => {
  let service: CreateAuthorCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CreateAuthorCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
