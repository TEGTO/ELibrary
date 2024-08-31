import { TestBed } from '@angular/core/testing';

import { AuthorServiceControllerService } from './author-controller.service';

describe('AuthorServiceControllerService', () => {
  let service: AuthorServiceControllerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthorServiceControllerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
