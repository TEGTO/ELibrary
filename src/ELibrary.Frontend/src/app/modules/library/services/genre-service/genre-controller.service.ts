import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { genreActions, selectGenreAmount, selectGenres } from '../..';
import { CreateGenreRequest, GenreApiService, GenreResponse, PaginatedRequest, UpdateGenreRequest } from '../../../shared';
import { GenreService } from './genre-service';

@Injectable({
  providedIn: 'root'
})
export class GenreServiceControllerService implements GenreService {

  constructor(
    private readonly apiService: GenreApiService,
    private readonly store: Store,
  ) { }

  getGenreById(id: number): Observable<GenreResponse> {
    return this.apiService.getById(id);
  }
  getGenresPaginated(request: PaginatedRequest): Observable<GenreResponse[]> {
    this.store.dispatch(genreActions.getPaginated({ request }));
    return this.store.select(selectGenres);
  }
  getItemTotalAmount(): Observable<number> {
    this.store.dispatch(genreActions.getTotalAmount());
    return this.store.select(selectGenreAmount);
  }
  createGenre(request: CreateGenreRequest): void {
    this.store.dispatch(genreActions.create({ request: request }));
  }
  updateGenre(request: UpdateGenreRequest): void {
    this.store.dispatch(genreActions.update({ request: request }));
  }
  deleteGenreById(id: number): void {
    this.store.dispatch(genreActions.deleteById({ id: id }));
  }
}