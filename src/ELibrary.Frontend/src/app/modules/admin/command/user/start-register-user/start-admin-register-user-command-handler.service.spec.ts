import { TestBed } from '@angular/core/testing';

import { StartAdminRegisterUserCommandHandlerService } from './start-admin-register-user-command-handler.service';

describe('StartAdminRegisterUserCommandHandlerService', () => {
  let service: StartAdminRegisterUserCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StartAdminRegisterUserCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
