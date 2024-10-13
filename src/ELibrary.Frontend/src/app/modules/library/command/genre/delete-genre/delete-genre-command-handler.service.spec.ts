import { TestBed } from '@angular/core/testing';

import { DeleteGenreCommandHandlerService } from './delete-genre-command-handler.service';

describe('DeleteGenreCommandHandlerService', () => {
  let service: DeleteGenreCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DeleteGenreCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
