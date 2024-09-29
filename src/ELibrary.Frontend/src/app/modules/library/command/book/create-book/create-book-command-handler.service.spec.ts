import { TestBed } from '@angular/core/testing';

import { CreateBookCommandHandlerService } from './create-book-command-handler.service';

describe('CreateBookCommandHandlerService', () => {
  let service: CreateBookCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CreateBookCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
