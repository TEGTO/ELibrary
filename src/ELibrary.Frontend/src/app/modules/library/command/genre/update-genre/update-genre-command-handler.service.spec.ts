import { TestBed } from '@angular/core/testing';

import { UpdateGenreCommandHandlerService } from './update-genre-command-handler.service';

describe('UpdateGenreCommandHandlerService', () => {
  let service: UpdateGenreCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UpdateGenreCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
