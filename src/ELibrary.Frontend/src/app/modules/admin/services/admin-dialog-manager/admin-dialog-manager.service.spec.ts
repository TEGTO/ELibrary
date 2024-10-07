import { TestBed } from '@angular/core/testing';

import { AdminDialogManagerService } from './admin-dialog-manager.service';

describe('AdminDialogManagerService', () => {
  let service: AdminDialogManagerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AdminDialogManagerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
