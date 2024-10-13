import { TestBed } from '@angular/core/testing';

import { AdminRegisterUserCommandHandlerService } from './admin-register-user-command-handler.service';

describe('AdminRegisterUserCommandHandlerService', () => {
  let service: AdminRegisterUserCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AdminRegisterUserCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
