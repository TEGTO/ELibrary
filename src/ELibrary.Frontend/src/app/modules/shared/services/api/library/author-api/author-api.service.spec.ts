import { TestBed } from '@angular/core/testing';

import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Author, CreateAuthorRequest, PaginatedRequest, UpdateAuthorRequest, URLDefiner } from '../../../..';
import { AuthorApiService } from './author-api.service';

describe('AuthorApiService', () => {
  let service: AuthorApiService;
  let httpTestingController: HttpTestingController;
  let mockUrlDefiner: jasmine.SpyObj<URLDefiner>;

  beforeEach(() => {
    mockUrlDefiner = jasmine.createSpyObj<URLDefiner>('URLDefiner', ['combineWithLibraryApiUrl']);
    mockUrlDefiner.combineWithLibraryApiUrl.and.callFake((subpath: string) => `/api${subpath}`);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        AuthorApiService,
        { provide: URLDefiner, useValue: mockUrlDefiner }
      ]
    });

    service = TestBed.inject(AuthorApiService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get author by id', () => {
    const expectedReq = `/api/author/1`;
    const response: Author = {
      id: 1,
      name: 'John',
      lastName: 'Doe',
      dateOfBirth: new Date('1980-01-01')
    };

    service.getById(1).subscribe(res => {
      expect(res).toEqual(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('GET');
    expect(mockUrlDefiner.combineWithLibraryApiUrl).toHaveBeenCalledWith('/author/1');
    req.flush(response);
  });

  it('should get paginated authors', () => {
    const expectedReq = `/api/author/pagination`;
    const request: PaginatedRequest = {
      pageNumber: 1,
      pageSize: 10
    };
    const response: Author[] = [
      { id: 1, name: 'John', lastName: 'Doe', dateOfBirth: new Date('1980-01-01') },
      { id: 2, name: 'Jane', lastName: 'Smith', dateOfBirth: new Date('1990-05-15') }
    ];

    service.getPaginated(request).subscribe(res => {
      expect(res).toEqual(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('POST');
    expect(mockUrlDefiner.combineWithLibraryApiUrl).toHaveBeenCalledWith('/author/pagination');
    req.flush(response);
  });

  it('should get the total number of authors', () => {
    const expectedReq = `/api/author/amount`;
    const response = 100;

    service.getItemTotalAmount().subscribe(res => {
      expect(res).toEqual(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('GET');
    expect(mockUrlDefiner.combineWithLibraryApiUrl).toHaveBeenCalledWith('/author/amount');
    req.flush(response);
  });

  it('should create a new author', () => {
    const expectedReq = `/api/author`;
    const request: CreateAuthorRequest = {
      name: 'John',
      lastName: 'Doe',
      dateOfBirth: new Date('1980-01-01')
    };
    const response: Author = {
      id: 1,
      name: 'John',
      lastName: 'Doe',
      dateOfBirth: new Date('1980-01-01')
    };

    service.create(request).subscribe(res => {
      expect(res).toEqual(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('POST');
    expect(mockUrlDefiner.combineWithLibraryApiUrl).toHaveBeenCalledWith('/author');
    req.flush(response);
  });

  it('should update an existing author', () => {
    const expectedReq = `/api/author`;
    const request: UpdateAuthorRequest = {
      id: 1,
      name: 'John',
      lastName: 'Doe',
      dateOfBirth: new Date('1980-01-01')
    };
    const response: Author = {
      id: 1,
      name: 'John',
      lastName: 'Doe',
      dateOfBirth: new Date('1980-01-01')
    };

    service.update(request).subscribe(res => {
      expect(res).toEqual(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('PUT');
    expect(mockUrlDefiner.combineWithLibraryApiUrl).toHaveBeenCalledWith('/author');
    req.flush(response);
  });

  it('should delete an author by id', () => {
    const expectedReq = `/api/author/1`;

    service.deleteById(1).subscribe(res => {
      expect(res).toBeNull();
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('DELETE');
    expect(mockUrlDefiner.combineWithLibraryApiUrl).toHaveBeenCalledWith('/author/1');
    req.flush(null);
  });

  it('should handle error on getById', () => {
    const expectedReq = `/api/author/1`;

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