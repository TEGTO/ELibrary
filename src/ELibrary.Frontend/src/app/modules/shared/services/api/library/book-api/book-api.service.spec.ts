import { TestBed } from '@angular/core/testing';

import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Book, CreateBookRequest, UpdateBookRequest, URLDefiner } from '../../../..';
import { BookApiService } from './book-api.service';

describe('BookApiService', () => {
  let service: BookApiService;
  let httpTestingController: HttpTestingController;
  let mockUrlDefiner: jasmine.SpyObj<URLDefiner>;

  beforeEach(() => {
    mockUrlDefiner = jasmine.createSpyObj<URLDefiner>('URLDefiner', ['combineWithLibraryApiUrl']);
    mockUrlDefiner.combineWithLibraryApiUrl.and.callFake((subpath: string) => `/api${subpath}`);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        BookApiService,
        { provide: URLDefiner, useValue: mockUrlDefiner }
      ]
    });

    service = TestBed.inject(BookApiService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get book by id', () => {
    const expectedReq = `/api/book/1`;
    const response: Book = {
      id: 1,
      name: 'The Great Gatsby',
      publicationDate: new Date('1925-04-10'),
      author: { id: 1, name: 'F. Scott Fitzgerald', lastName: 'Fitzgerald', dateOfBirth: new Date('1896-09-24') },
      genre: { id: 1, name: 'Classic' }
    };

    service.getById(1).subscribe(res => {
      expect(res).toEqual(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('GET');
    expect(mockUrlDefiner.combineWithLibraryApiUrl).toHaveBeenCalledWith('/book/1');
    req.flush(response);
  });

  it('should get paginated books', () => {
    const expectedReq = `/api/book/pagination`;
    const request = { pageNumber: 1, pageSize: 10 };
    const response: Book[] = [
      { id: 1, name: 'The Great Gatsby', publicationDate: new Date('1925-04-10'), author: { id: 1, name: 'F. Scott Fitzgerald', lastName: 'Fitzgerald', dateOfBirth: new Date('1896-09-24') }, genre: { id: 1, name: 'Classic' } },
      { id: 2, name: 'Moby Dick', publicationDate: new Date('1851-11-14'), author: { id: 2, name: 'Herman Melville', lastName: 'Melville', dateOfBirth: new Date('1819-08-01') }, genre: { id: 2, name: 'Adventure' } }
    ];

    service.getPaginated(request).subscribe(res => {
      expect(res).toEqual(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('POST');
    expect(mockUrlDefiner.combineWithLibraryApiUrl).toHaveBeenCalledWith('/book/pagination');
    req.flush(response);
  });

  it('should get the total number of books', () => {
    const expectedReq = `/api/book/amount`;
    const response = 100;

    service.getItemTotalAmount().subscribe(res => {
      expect(res).toEqual(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('GET');
    expect(mockUrlDefiner.combineWithLibraryApiUrl).toHaveBeenCalledWith('/book/amount');
    req.flush(response);
  });

  it('should create a new book', () => {
    const expectedReq = `/api/book`;
    const request: CreateBookRequest = {
      name: '1984',
      publicationDate: new Date('1949-06-08'),
      authorId: 1,
      genreId: 1
    };
    const response: Book = {
      id: 1,
      name: '1984',
      publicationDate: new Date('1949-06-08'),
      author: { id: 1, name: 'George Orwell', lastName: 'Orwell', dateOfBirth: new Date('1903-06-25') },
      genre: { id: 1, name: 'Dystopian' }
    };

    service.create(request).subscribe(res => {
      expect(res).toEqual(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('POST');
    expect(mockUrlDefiner.combineWithLibraryApiUrl).toHaveBeenCalledWith('/book');
    req.flush(response);
  });

  it('should update an existing book', () => {
    const expectedReq = `/api/book`;
    const request: UpdateBookRequest = {
      id: 1,
      name: '1984',
      publicationDate: new Date('1949-06-08'),
      authorId: 1,
      genreId: 1
    };
    const response: Book = {
      id: 1,
      name: '1984',
      publicationDate: new Date('1949-06-08'),
      author: { id: 1, name: 'George Orwell', lastName: 'Orwell', dateOfBirth: new Date('1903-06-25') },
      genre: { id: 1, name: 'Dystopian' }
    };

    service.update(request).subscribe(res => {
      expect(res).toEqual(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('PUT');
    expect(mockUrlDefiner.combineWithLibraryApiUrl).toHaveBeenCalledWith('/book');
    req.flush(response);
  });

  it('should delete a book by id', () => {
    const expectedReq = `/api/book/1`;

    service.deleteById(1).subscribe(res => {
      expect(res).toBeNull();
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('DELETE');
    expect(mockUrlDefiner.combineWithLibraryApiUrl).toHaveBeenCalledWith('/book/1');
    req.flush(null);
  });

  it('should handle error on getById', () => {
    const expectedReq = `/api/book/1`;

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