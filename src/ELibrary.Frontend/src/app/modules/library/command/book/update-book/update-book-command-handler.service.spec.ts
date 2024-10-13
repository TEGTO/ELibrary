import { TestBed } from '@angular/core/testing';

import { UpdateBookCommandHandlerService } from './update-book-command-handler.service';

describe('UpdateBookCommandHandlerService', () => {
  let service: UpdateBookCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UpdateBookCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
