import { TestBed } from '@angular/core/testing';

import { StartOAuthLoginCommandHandlerService } from './start-oauth-login-command-handler.service';

describe('StartOAuthLoginCommandHandlerService', () => {
  let service: StartOAuthLoginCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StartOAuthLoginCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
