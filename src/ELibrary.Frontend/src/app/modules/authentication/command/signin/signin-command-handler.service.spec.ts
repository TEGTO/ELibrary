import { TestBed } from '@angular/core/testing';

import { SignInCommandHandlerService } from './signin-command-handler.service';

describe('SigninCommandHandlerService', () => {
  let service: SignInCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SignInCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
