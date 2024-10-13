import { TestBed } from '@angular/core/testing';

import { StartAddingClientCommandHandlerService } from './start-adding-client-command-handler.service';

describe('StartAddingClientCommandHandlerService', () => {
  let service: StartAddingClientCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StartAddingClientCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
