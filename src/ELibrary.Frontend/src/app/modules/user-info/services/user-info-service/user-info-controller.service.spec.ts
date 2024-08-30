import { TestBed } from '@angular/core/testing';

import { UserInfoController } from './user-info-controller.service';

describe('UserInfoControllerService', () => {
  let service: UserInfoController;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserInfoController);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
