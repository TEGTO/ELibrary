import { TestBed } from '@angular/core/testing';

import { GenreControllerService } from './genre-controller.service';

describe('GenreControllerService', () => {
  let service: GenreControllerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GenreControllerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
