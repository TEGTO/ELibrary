import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { Book, BookFilterRequest, CreateBookRequest, getDefaultAuthor, getDefaultBook, getDefaultGenre, getDefaultPublisher, UpdateBookRequest, URLDefiner } from '../../../..';
import { BookApiService } from './book-api.service';

describe('BookApiService', () => {
  let service: BookApiService;
  let httpTestingController: HttpTestingController;
  let mockUrlDefiner: jasmine.SpyObj<URLDefiner>;

  beforeEach(() => {
    mockUrlDefiner = jasmine.createSpyObj<URLDefiner>('URLDefiner', ['combineWithLibraryApiUrl']);
    mockUrlDefiner.combineWithLibraryApiUrl.and.callFake((subpath: string) => `/api${subpath}`);

    TestBed.configureTestingModule({
      providers: [
        BookApiService,
        { provide: URLDefiner, useValue: mockUrlDefiner },
        provideHttpClient(),
        provideHttpClientTesting()
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
      name: '1984',
      price: 100,
      coverType: 0,
      pageAmount: 100,
      coverImgUrl: "",
      stockAmount: 100,
      publicationDate: new Date('1949-06-08'),
      author: getDefaultAuthor(),
      genre: getDefaultGenre(),
      publisher: getDefaultPublisher(),
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
    const request: BookFilterRequest = {
      pageNumber: 1,
      pageSize: 10,
      containsName: "",
      publicationFrom: null,
      publicationTo: null,
      minPrice: null,
      maxPrice: null,
      coverType: null,
      onlyInStock: null,
      minPageAmount: null,
      maxPageAmount: null,
      authorId: null,
      genreId: null,
      publisherId: null,
      sorting: null,
    }
    const response: Book[] = [
      getDefaultBook(),
      getDefaultBook(),
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
    const getItemReq: BookFilterRequest = {
      pageNumber: 1,
      pageSize: 10,
      containsName: "",
      publicationFrom: null,
      publicationTo: null,
      minPrice: null,
      maxPrice: null,
      coverType: null,
      onlyInStock: null,
      minPageAmount: null,
      maxPageAmount: null,
      authorId: null,
      genreId: null,
      publisherId: null,
      sorting: null,
    }

    service.getItemTotalAmount(getItemReq).subscribe(res => {
      expect(res).toEqual(response);
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('POST');
    expect(mockUrlDefiner.combineWithLibraryApiUrl).toHaveBeenCalledWith('/book/amount');
    req.flush(response);
  });

  it('should create a new book', () => {
    const expectedReq = `/api/book`;
    const request: CreateBookRequest = {
      name: '1984',
      publicationDate: new Date('1949-06-08'),
      price: 100,
      coverType: 0,
      pageAmount: 100,
      coverImgUrl: "",
      publisherId: 1,
      authorId: 1,
      genreId: 1,
    };
    const response: Book = {
      id: 1,
      name: '1984',
      price: 100,
      coverType: 0,
      pageAmount: 100,
      coverImgUrl: "",
      stockAmount: 100,
      publicationDate: new Date('1949-06-08'),
      author: getDefaultAuthor(),
      genre: getDefaultGenre(),
      publisher: getDefaultPublisher(),
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
      price: 100,
      coverType: 0,
      pageAmount: 100,
      coverImgUrl: "",
      publisherId: 1,
      authorId: 1,
      genreId: 1,
    };
    const response: Book = {
      id: 1,
      name: '1984',
      price: 100,
      coverType: 0,
      pageAmount: 100,
      coverImgUrl: "",
      stockAmount: 100,
      publicationDate: new Date('1949-06-08'),
      author: getDefaultAuthor(),
      genre: getDefaultGenre(),
      publisher: getDefaultPublisher(),
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
      expect(res.body).toBeNull();
    });

    const req = httpTestingController.expectOne(expectedReq);
    expect(req.request.method).toBe('DELETE');
    expect(mockUrlDefiner.combineWithLibraryApiUrl).toHaveBeenCalledWith('/book/1');
    req.flush(null);
  });

  it('should handle error on getById', () => {
    const expectedReq = `/api/book/1`;

    service.getById(1).subscribe({
      next: () => fail('Expected an error, not a success'),
      error: (error) => {
        expect(error).toBeTruthy();
      }
    });

    const req = httpTestingController.expectOne(expectedReq);
    req.flush('Error', { status: 404, statusText: 'Not Found' });
  });
});