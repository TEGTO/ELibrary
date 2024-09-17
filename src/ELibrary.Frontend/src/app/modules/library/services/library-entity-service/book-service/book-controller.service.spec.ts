import { TestBed } from '@angular/core/testing';
import { Store } from '@ngrx/store';
import { of } from 'rxjs';
import { bookActions, selectBookAmount, selectBooks } from '../..';
import { BookApiService, BookResponse, CreateBookRequest, PaginatedRequest, UpdateBookRequest } from '../../../shared';
import { BookControllerService } from './book-controller.service';

describe('BookControllerService', () => {
  let service: BookControllerService;
  let store: jasmine.SpyObj<Store>;
  let apiService: jasmine.SpyObj<BookApiService>;

  const mockBookData: BookResponse[] = [
    { id: 1, name: 'Book One', publicationDate: new Date('2022-01-01'), author: { id: 1, name: 'John', lastName: 'Doe', dateOfBirth: new Date('1980-01-01') }, genre: { id: 1, name: 'Action' } },
    { id: 2, name: 'Book Two', publicationDate: new Date('2022-02-02'), author: { id: 2, name: 'Jane', lastName: 'Smith', dateOfBirth: new Date('1990-02-02') }, genre: { id: 2, name: 'Comedy' } }
  ];

  const mockTotalAmount: number = 15;

  beforeEach(() => {
    const storeSpy = jasmine.createSpyObj('Store', ['dispatch', 'select']);
    const apiServiceSpy = jasmine.createSpyObj('BookApiService', ['getById']);

    TestBed.configureTestingModule({
      providers: [
        BookControllerService,
        { provide: Store, useValue: storeSpy },
        { provide: BookApiService, useValue: apiServiceSpy }
      ]
    });

    service = TestBed.inject(BookControllerService);
    store = TestBed.inject(Store) as jasmine.SpyObj<Store>;
    apiService = TestBed.inject(BookApiService) as jasmine.SpyObj<BookApiService>;

    store.select.and.callFake((selector: any) => {
      if (selector === selectBooks) {
        return of(mockBookData);
      } else if (selector === selectBookAmount) {
        return of(mockTotalAmount);
      } else {
        return of(null);
      }
    });
  });

  it('should return book data by ID', (done) => {
    const bookId = 1;
    const expectedBook: BookResponse = mockBookData[0];
    apiService.getById.and.returnValue(of(expectedBook));

    service.getById(bookId).subscribe(result => {
      expect(apiService.getById).toHaveBeenCalledWith(bookId);
      expect(result).toEqual(expectedBook);
      done();
    });
  });

  it('should dispatch getPaginated action and return paginated books', (done) => {
    const request: PaginatedRequest = { pageNumber: 1, pageSize: 10 };
    store.select.and.returnValue(of(mockBookData));

    service.getPaginated(request).subscribe(result => {
      expect(store.dispatch).toHaveBeenCalledWith(bookActions.getPaginated({ request }));
      expect(result).toEqual(mockBookData);
      done();
    });
  });

  it('should dispatch getTotalAmount action and return total amount of books', (done) => {
    store.select.and.returnValue(of(mockTotalAmount));

    service.getItemTotalAmount().subscribe(result => {
      expect(store.dispatch).toHaveBeenCalledWith(bookActions.getTotalAmount());
      expect(result).toEqual(mockTotalAmount);
      done();
    });
  });

  it('should dispatch create action for a new book', () => {
    const newBook: CreateBookRequest = { name: 'New Book', publicationDate: new Date('2023-03-03'), authorId: 3, genreId: 3 };

    service.create(newBook);

    expect(store.dispatch).toHaveBeenCalledWith(bookActions.create({ request: newBook }));
  });

  it('should dispatch update action for an existing book', () => {
    const updatedBook: UpdateBookRequest = { id: 1, name: 'Updated Book', publicationDate: new Date('2023-03-03'), authorId: 3, genreId: 3 };

    service.update(updatedBook);

    expect(store.dispatch).toHaveBeenCalledWith(bookActions.update({ request: updatedBook }));
  });

  it('should dispatch deleteById action for a book by ID', () => {
    const bookId = 1;

    service.deleteById(bookId);

    expect(store.dispatch).toHaveBeenCalledWith(bookActions.deleteById({ id: bookId }));
  });
});