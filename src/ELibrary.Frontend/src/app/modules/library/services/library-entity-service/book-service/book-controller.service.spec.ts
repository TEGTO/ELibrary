/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { Store } from '@ngrx/store';
import { of } from 'rxjs';
import { bookActions, selectBookAmount, selectBooks } from '../../..';
import { Book, BookApiService, BookFilterRequest, BookResponse, CreateBookRequest, getDefaultBook, defaultBookFilterRequest as getDefaultBookFilterRequest, mapBookToCreateBookRequest, mapBookToUpdateBookRequest, UpdateBookRequest } from '../../../../shared';
import { BookControllerService } from './book-controller.service';

describe('BookControllerService', () => {
    let service: BookControllerService;
    let store: jasmine.SpyObj<Store>;
    let apiService: jasmine.SpyObj<BookApiService>;

    const mockBookData: Book[] = [
        { ...getDefaultBook(), id: 1 },
        { ...getDefaultBook(), id: 2 },
    ];

    const mockTotalAmount = 15;

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
        const request: BookFilterRequest = {
            ...getDefaultBookFilterRequest(),
            pageNumber: 1,
            pageSize: 10,
        };
        store.select.and.returnValue(of(mockBookData));

        service.getPaginated(request).subscribe(result => {
            expect(store.dispatch).toHaveBeenCalledWith(bookActions.getPaginated({ request }));
            expect(result).toEqual(mockBookData);
            done();
        });
    });

    it('should dispatch getTotalAmount action and return total amount of books', (done) => {
        store.select.and.returnValue(of(mockTotalAmount));
        const request: BookFilterRequest = {
            ...getDefaultBookFilterRequest(),
            pageNumber: 1,
            pageSize: 10,
        };

        service.getItemTotalAmount(request).subscribe(result => {
            expect(store.dispatch).toHaveBeenCalledWith(bookActions.getTotalAmount({ request: request }));
            expect(result).toEqual(mockTotalAmount);
            done();
        });
    });

    it('should dispatch create action for a new book', () => {
        const newBook: CreateBookRequest = mapBookToCreateBookRequest(getDefaultBook());

        service.create(newBook);

        expect(store.dispatch).toHaveBeenCalledWith(bookActions.create({ request: newBook }));
    });

    it('should dispatch update action for an existing book', () => {
        const updatedBook: UpdateBookRequest = mapBookToUpdateBookRequest(getDefaultBook());

        service.update(updatedBook);

        expect(store.dispatch).toHaveBeenCalledWith(bookActions.update({ request: updatedBook }));
    });

    it('should dispatch deleteById action for a book by ID', () => {
        const bookId = 1;

        service.deleteById(bookId);

        expect(store.dispatch).toHaveBeenCalledWith(bookActions.deleteById({ id: bookId }));
    });
});