import { TestBed } from '@angular/core/testing';

import { StartRegistrationCommandHandlerService } from './start-registration-command-handler.service';

describe('StartRegistrationCommandHandlerService', () => {
  let service: StartRegistrationCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StartRegistrationCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
