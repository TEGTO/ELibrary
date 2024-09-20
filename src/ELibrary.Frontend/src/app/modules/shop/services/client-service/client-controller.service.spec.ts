import { TestBed } from '@angular/core/testing';

import { ClientControllerService } from './client-controller.service';

describe('ClientControllerService', () => {
  let service: ClientControllerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ClientControllerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
