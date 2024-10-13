import { TestBed } from '@angular/core/testing';

import { UpdateAuthorCommandHandlerService } from './update-author-command-handler.service';

describe('UpdateAuthorCommandHandlerService', () => {
  let service: UpdateAuthorCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UpdateAuthorCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
