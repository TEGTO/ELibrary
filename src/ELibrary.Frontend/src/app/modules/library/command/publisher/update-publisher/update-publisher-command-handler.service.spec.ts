import { TestBed } from '@angular/core/testing';

import { UpdatePublisherCommandHandlerService } from './update-publisher-command-handler.service';

describe('UpdatePublisherCommandHandlerService', () => {
  let service: UpdatePublisherCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UpdatePublisherCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
