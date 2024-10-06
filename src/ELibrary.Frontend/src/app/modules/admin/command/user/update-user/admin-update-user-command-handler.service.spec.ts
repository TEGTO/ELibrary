import { TestBed } from '@angular/core/testing';

import { AdminUpdateUserCommandHandlerService } from './admin-update-user-command-handler.service';

describe('AdminUpdateUserCommandHandlerService', () => {
  let service: AdminUpdateUserCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AdminUpdateUserCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
