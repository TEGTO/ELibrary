import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { genreActions, selectGenreAmount, selectGenres } from '../..';
import { CreateGenreRequest, GenreApiService, GenreResponse, LibraryFilterRequest, UpdateGenreRequest } from '../../../shared';
import { GenreService } from './genre-service';

@Injectable({
  providedIn: 'root'
})
export class GenreControllerService implements GenreService {

  constructor(
    private readonly apiService: GenreApiService,
    private readonly store: Store,
  ) { }

  getById(id: number): Observable<GenreResponse> {
    return this.apiService.getById(id);
  }
  getPaginated(request: LibraryFilterRequest): Observable<GenreResponse[]> {
    this.store.dispatch(genreActions.getPaginated({ request: request }));
    return this.store.select(selectGenres);
  }
  getItemTotalAmount(request: LibraryFilterRequest): Observable<number> {
    this.store.dispatch(genreActions.getTotalAmount({ request: request }));
    return this.store.select(selectGenreAmount);
  }
  create(request: CreateGenreRequest): void {
    this.store.dispatch(genreActions.create({ request: request }));
  }
  update(request: UpdateGenreRequest): void {
    this.store.dispatch(genreActions.update({ request: request }));
  }
  deleteById(id: number): void {
    this.store.dispatch(genreActions.deleteById({ id: id }));
  }
}