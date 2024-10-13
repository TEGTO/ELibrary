import { TestBed } from '@angular/core/testing';

import { AdminCreateClientCommandHandlerService } from './admin-create-client-command-handler.service';

describe('AdminCreateClientCommandHandlerService', () => {
  let service: AdminCreateClientCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AdminCreateClientCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
