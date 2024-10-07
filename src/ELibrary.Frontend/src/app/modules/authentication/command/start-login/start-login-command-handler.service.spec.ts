import { TestBed } from '@angular/core/testing';

import { StartLoginCommandHandlerService } from './start-login-command-handler.service';

describe('StartLoginCommandHandlerService', () => {
  let service: StartLoginCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StartLoginCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
