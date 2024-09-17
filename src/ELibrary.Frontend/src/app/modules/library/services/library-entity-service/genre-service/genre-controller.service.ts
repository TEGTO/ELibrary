import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { genreActions, selectGenreAmount, selectGenres } from '../../..';
import { CreateGenreRequest, GenreApiService, GenreResponse, LibraryFilterRequest, UpdateGenreRequest } from '../../../../shared';
import { BaseControllerService } from '../base-entity-service/base-entity.service';
import { GenreService } from './genre-service';

@Injectable({
  providedIn: 'root'
})
export class GenreControllerService extends BaseControllerService<
  GenreResponse,
  LibraryFilterRequest,
  CreateGenreRequest,
  UpdateGenreRequest
> implements GenreService {
  constructor(
    apiService: GenreApiService,
    store: Store
  ) {
    super(apiService, store, genreActions, {
      selectItems: selectGenres,
      selectTotalAmount: selectGenreAmount
    });
  }
}