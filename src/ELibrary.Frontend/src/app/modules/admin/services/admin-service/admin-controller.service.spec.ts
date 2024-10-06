import { TestBed } from '@angular/core/testing';

import { AdminControllerService } from './admin-controller.service';

describe('AdminControllerService', () => {
  let service: AdminControllerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AdminControllerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
