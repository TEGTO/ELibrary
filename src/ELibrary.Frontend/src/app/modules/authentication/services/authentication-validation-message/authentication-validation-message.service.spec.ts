import { TestBed } from '@angular/core/testing';

import { AuthenticationValidationMessageService } from './authentication-validation-message.service';

describe('AuthenticationValidationMessageService', () => {
  let service: AuthenticationValidationMessageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthenticationValidationMessageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
