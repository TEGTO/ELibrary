import { TestBed } from '@angular/core/testing';

import { SendAdvisorMessageCommandHandlerService } from './send-advisor-message-command-handler.service';

describe('SendAdvisorMessageCommandHandlerService', () => {
  let service: SendAdvisorMessageCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SendAdvisorMessageCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
