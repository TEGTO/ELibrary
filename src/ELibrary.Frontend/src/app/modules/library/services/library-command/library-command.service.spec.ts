import { TestBed } from '@angular/core/testing';

import { LibraryCommandService } from './library-command.service';

describe('LibraryCommandService', () => {
  let service: LibraryCommandService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LibraryCommandService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
