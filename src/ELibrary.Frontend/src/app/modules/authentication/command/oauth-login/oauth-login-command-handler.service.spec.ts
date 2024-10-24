import { TestBed } from '@angular/core/testing';

import { OAuthLoginCommandHandlerService } from './oauth-login-command-handler.service';

describe('OAuthLoginCommandHandlerService', () => {
  let service: OAuthLoginCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OAuthLoginCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
