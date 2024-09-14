import { TestBed } from '@angular/core/testing';

import { AuthenticationCommandService } from './authentication-command.service';

describe('AuthenticationCommandService', () => {
  let service: AuthenticationCommandService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthenticationCommandService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
