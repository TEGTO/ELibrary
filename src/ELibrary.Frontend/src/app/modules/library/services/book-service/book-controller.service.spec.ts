import { TestBed } from '@angular/core/testing';

import { BookServiceControllerService } from './book-controller.service';

describe('BookServiceControllerService', () => {
  let service: BookServiceControllerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BookServiceControllerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
