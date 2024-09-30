/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { Store } from '@ngrx/store';
import { of } from 'rxjs';
import { authorActions, selectAuthorAmount, selectAuthors } from '../../..';
import { AuthorApiService, AuthorResponse, CreateAuthorRequest, LibraryFilterRequest, UpdateAuthorRequest } from '../../../../shared';
import { AuthorControllerService } from './author-controller.service';

describe('AuthorControllerService', () => {
  let service: AuthorControllerService;
  let store: jasmine.SpyObj<Store>;
  let apiService: jasmine.SpyObj<AuthorApiService>;

  const mockAuthorData: AuthorResponse[] = [
    { id: 1, name: 'John', lastName: 'Doe', dateOfBirth: new Date('1980-01-01') },
    { id: 2, name: 'Jane', lastName: 'Smith', dateOfBirth: new Date('1990-02-02') }
  ];

  const mockTotalAmount = 5;

  beforeEach(() => {
    const storeSpy = jasmine.createSpyObj('Store', ['dispatch', 'select']);
    const apiServiceSpy = jasmine.createSpyObj('AuthorApiService', ['getById']);

    TestBed.configureTestingModule({
      providers: [
        AuthorControllerService,
        { provide: Store, useValue: storeSpy },
        { provide: AuthorApiService, useValue: apiServiceSpy }
      ]
    });

    service = TestBed.inject(AuthorControllerService);
    store = TestBed.inject(Store) as jasmine.SpyObj<Store>;
    apiService = TestBed.inject(AuthorApiService) as jasmine.SpyObj<AuthorApiService>;

    store.select.and.callFake((selector: any) => {
      if (selector === selectAuthors) {
        return of(mockAuthorData);
      } else if (selector === selectAuthorAmount) {
        return of(mockTotalAmount);
      } else {
        return of(null);
      }
    });
  });

  it('should return author data by ID', (done) => {
    const authorId = 1;
    const expectedAuthor: AuthorResponse = { id: authorId, name: 'John', lastName: 'Doe', dateOfBirth: new Date('1980-01-01') };
    apiService.getById.and.returnValue(of(expectedAuthor));

    service.getById(authorId).subscribe(result => {
      expect(apiService.getById).toHaveBeenCalledWith(authorId);
      expect(result).toEqual(expectedAuthor);
      done();
    });
  });

  it('should dispatch getPaginated action and return paginated authors', (done) => {
    const request: LibraryFilterRequest = { containsName: "", pageNumber: 1, pageSize: 10 };
    store.select.and.returnValue(of(mockAuthorData));

    service.getPaginated(request).subscribe(result => {
      expect(store.dispatch).toHaveBeenCalledWith(authorActions.getPaginated({ request: request }));
      expect(result).toEqual(mockAuthorData);
      done();
    });
  });

  it('should dispatch getTotalAmount action and return total amount of authors', (done) => {
    store.select.and.returnValue(of(mockTotalAmount));
    const request: LibraryFilterRequest = { containsName: "", pageNumber: 1, pageSize: 10 };

    service.getItemTotalAmount(request).subscribe(result => {
      expect(store.dispatch).toHaveBeenCalledWith(authorActions.getTotalAmount({ request: request }));
      expect(result).toEqual(mockTotalAmount);
      done();
    });
  });

  it('should dispatch create action for a new author', () => {
    const newAuthor: CreateAuthorRequest = { name: 'Alice', lastName: 'Johnson', dateOfBirth: new Date('1995-03-03') };

    service.create(newAuthor);

    expect(store.dispatch).toHaveBeenCalledWith(authorActions.create({ request: newAuthor }));
  });

  it('should dispatch update action for an existing author', () => {
    const updatedAuthor: UpdateAuthorRequest = { id: 1, name: 'John', lastName: 'Doe', dateOfBirth: new Date('1980-01-01') };

    service.update(updatedAuthor);

    expect(store.dispatch).toHaveBeenCalledWith(authorActions.update({ request: updatedAuthor }));
  });

  it('should dispatch deleteById action for an author by ID', () => {
    const authorId = 1;

    service.deleteById(authorId);

    expect(store.dispatch).toHaveBeenCalledWith(authorActions.deleteById({ id: authorId }));
  });
});