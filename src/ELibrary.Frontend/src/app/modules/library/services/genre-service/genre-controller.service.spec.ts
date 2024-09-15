import { TestBed } from '@angular/core/testing';

import { Store } from '@ngrx/store';
import { of } from 'rxjs';
import { genreActions, selectGenreAmount, selectGenres } from '../..';
import { CreateGenreRequest, GenreApiService, GenreResponse, PaginatedRequest, UpdateGenreRequest } from '../../../shared';
import { GenreControllerService } from './genre-controller.service';

describe('GenreControllerService', () => {
  let service: GenreControllerService;
  let store: jasmine.SpyObj<Store>;
  let apiService: jasmine.SpyObj<GenreApiService>;

  const mockGenreData: GenreResponse[] = [
    { id: 1, name: 'Action' },
    { id: 2, name: 'Comedy' }
  ];

  const mockTotalAmount: number = 10;

  beforeEach(() => {
    const storeSpy = jasmine.createSpyObj('Store', ['dispatch', 'select']);
    const apiServiceSpy = jasmine.createSpyObj('GenreApiService', ['getById']);

    TestBed.configureTestingModule({
      providers: [
        GenreControllerService,
        { provide: Store, useValue: storeSpy },
        { provide: GenreApiService, useValue: apiServiceSpy }
      ]
    });

    service = TestBed.inject(GenreControllerService);
    store = TestBed.inject(Store) as jasmine.SpyObj<Store>;
    apiService = TestBed.inject(GenreApiService) as jasmine.SpyObj<GenreApiService>;

    store.select.and.callFake((selector: any) => {
      if (selector === selectGenres) {
        return of(mockGenreData);
      } else if (selector === selectGenreAmount) {
        return of(mockTotalAmount);
      } else {
        return of(null);
      }
    });
  });

  it('should return genre data by ID', (done) => {
    const genreId = 1;
    const expectedGenre: GenreResponse = { id: genreId, name: 'Action' };
    apiService.getById.and.returnValue(of(expectedGenre));

    service.getById(genreId).subscribe(result => {
      expect(apiService.getById).toHaveBeenCalledWith(genreId);
      expect(result).toEqual(expectedGenre);
      done();
    });
  });

  it('should dispatch getPaginated action and return paginated genres', (done) => {
    const request: PaginatedRequest = { pageNumber: 1, pageSize: 10 };
    store.select.and.returnValue(of(mockGenreData));

    service.getPaginated(request).subscribe(result => {
      expect(store.dispatch).toHaveBeenCalledWith(genreActions.getPaginated({ request }));
      expect(result).toEqual(mockGenreData);
      done();
    });
  });

  it('should dispatch getTotalAmount action and return total amount of genres', (done) => {
    store.select.and.returnValue(of(mockTotalAmount));

    service.getItemTotalAmount().subscribe(result => {
      expect(store.dispatch).toHaveBeenCalledWith(genreActions.getTotalAmount());
      expect(result).toEqual(mockTotalAmount);
      done();
    });
  });

  it('should dispatch create action for a new genre', () => {
    const newGenre: CreateGenreRequest = { name: 'Drama' };

    service.create(newGenre);

    expect(store.dispatch).toHaveBeenCalledWith(genreActions.create({ request: newGenre }));
  });

  it('should dispatch update action for an existing genre', () => {
    const updatedGenre: UpdateGenreRequest = { id: 1, name: 'Adventure' };

    service.update(updatedGenre);

    expect(store.dispatch).toHaveBeenCalledWith(genreActions.update({ request: updatedGenre }));
  });

  it('should dispatch deleteById action for a genre by ID', () => {
    const genreId = 1;

    service.deleteById(genreId);

    expect(store.dispatch).toHaveBeenCalledWith(genreActions.deleteById({ id: genreId }));
  });
});