import { TestBed } from '@angular/core/testing';

import { LibraryDialogManagerService } from './library-dialog-manager.service';

describe('LibraryDialogManagerService', () => {
  let service: LibraryDialogManagerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LibraryDialogManagerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
