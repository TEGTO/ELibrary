import { TestBed } from '@angular/core/testing';

import { ChangeChatVisibilityCommandHandlerService } from './change-chat-visibility-command-handler.service';

describe('ChangeChatVisibilityCommandHandlerService', () => {
  let service: ChangeChatVisibilityCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChangeChatVisibilityCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
