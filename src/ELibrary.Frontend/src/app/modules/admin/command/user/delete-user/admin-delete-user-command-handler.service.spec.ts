import { TestBed } from '@angular/core/testing';

import { AdminDeleteUserCommandHandlerService } from './admin-delete-user-command-handler.service';

describe('AdminDeleteUserCommandHandlerService', () => {
  let service: AdminDeleteUserCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AdminDeleteUserCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
