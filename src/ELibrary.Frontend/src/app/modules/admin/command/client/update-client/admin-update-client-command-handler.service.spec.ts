import { TestBed } from '@angular/core/testing';

import { AdminUpdateClientCommandHandlerService } from './admin-update-client-command-handler.service';

describe('AdminUpdateClientCommandHandlerService', () => {
  let service: AdminUpdateClientCommandHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AdminUpdateClientCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
