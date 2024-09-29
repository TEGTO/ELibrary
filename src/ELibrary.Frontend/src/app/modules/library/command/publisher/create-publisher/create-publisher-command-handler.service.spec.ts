import { TestBed } from '@angular/core/testing';

import { CreatePublisherCommandHandlerService } from './create-publisher-command-handler.service';

describe('CreatePublisherCommandHandlerService', () => {
  let service: CreatePublisherCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CreatePublisherCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
