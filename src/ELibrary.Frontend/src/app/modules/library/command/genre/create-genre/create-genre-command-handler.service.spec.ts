import { TestBed } from '@angular/core/testing';

import { CreateGenreCommandHandlerService } from './create-genre-command-handler.service';

describe('CreateGenreCommandHandlerService', () => {
  let service: CreateGenreCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CreateGenreCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
