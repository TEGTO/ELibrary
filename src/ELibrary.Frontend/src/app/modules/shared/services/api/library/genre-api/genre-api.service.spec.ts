import { TestBed } from '@angular/core/testing';

import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { CreateGenreRequest, Genre, LibraryFilterRequest, UpdateGenreRequest, URLDefiner } from '../../../..';
import { GenreApiService } from './genre-api.service';

describe('GenreApiService', () => {
  let service: GenreApiService;
  let httpTestingController: HttpTestingController;
  let mockUrlDefiner: jasmine.SpyObj<URLDefiner>;

  beforeEach(() => {
    mockUrlDefiner = jasmine.createSpyObj<URLDefiner>('URLDefiner', ['combineWithLibraryApiUrl']);
    mockUrlDefiner.combineWithLibraryApiUrl.and.callFake((subpath: string) => `/api${subpath}`);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        GenreApiService,
        { provide: URLDefiner, useValue: mockUrlDefiner }
      ]
    });

    service = TestBed.inject(GenreApiService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get genre by id', () => {
    const expectedReq = `/api/genre/1`;
    const response: Genre = {
      id: 1,
      name: 'Fantasy'
    };

    service.getById(1).subscribe(res => {
      expect(res).toEqual(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('GET');
    expect(mockUrlDefiner.combineWithLibraryApiUrl).toHaveBeenCalledWith('/genre/1');
    req.flush(response);
  });

  it('should get paginated genres', () => {
    const expectedReq = `/api/genre/pagination`;
    const request: LibraryFilterRequest = { containsName: "", pageNumber: 1, pageSize: 10 };
    const response: Genre[] = [
      { id: 1, name: 'Fantasy' },
      { id: 2, name: 'Science Fiction' }
    ];

    service.getPaginated(request).subscribe(res => {
      expect(res).toEqual(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('POST');
    expect(mockUrlDefiner.combineWithLibraryApiUrl).toHaveBeenCalledWith('/genre/pagination');
    req.flush(response);
  });

  it('should get the total number of genres', () => {
    const expectedReq = `/api/genre/amount`;
    const request: LibraryFilterRequest = { containsName: "", pageNumber: 1, pageSize: 10 };
    const response = 50;

    service.getItemTotalAmount(request).subscribe(res => {
      expect(res).toEqual(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('POST');
    expect(mockUrlDefiner.combineWithLibraryApiUrl).toHaveBeenCalledWith('/genre/amount');
    req.flush(response);
  });

  it('should create a new genre', () => {
    const expectedReq = `/api/genre`;
    const request: CreateGenreRequest = {
      name: 'Mystery'
    };
    const response: Genre = {
      id: 1,
      name: 'Mystery'
    };

    service.create(request).subscribe(res => {
      expect(res).toEqual(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('POST');
    expect(mockUrlDefiner.combineWithLibraryApiUrl).toHaveBeenCalledWith('/genre');
    req.flush(response);
  });

  it('should update an existing genre', () => {
    const expectedReq = `/api/genre`;
    const request: UpdateGenreRequest = {
      id: 1,
      name: 'Historical Fiction'
    };
    const response: Genre = {
      id: 1,
      name: 'Mystery'
    };

    service.update(request).subscribe(res => {
      expect(res).toEqual(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('PUT');
    expect(mockUrlDefiner.combineWithLibraryApiUrl).toHaveBeenCalledWith('/genre');
    req.flush(response);
  });

  it('should delete a genre by id', () => {
    const expectedReq = `/api/genre/1`;

    service.deleteById(1).subscribe(res => {
      expect(res.body).toBeNull();
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('DELETE');
    expect(mockUrlDefiner.combineWithLibraryApiUrl).toHaveBeenCalledWith('/genre/1');
    req.flush(null);
  });

  it('should handle error on getById', () => {
    const expectedReq = `/api/genre/1`;

    service.getById(1).subscribe(
      () => fail('Expected an error, not a success'),
      (error) => {
        expect(error).toBeTruthy();
      }
    );

    const req = httpTestingController.expectOne(expectedReq);
    req.flush('Error', { status: 404, statusText: 'Not Found' });
  });
});