import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { authorActions, selectAuthors } from '../..';
import { AuthorApiService, AuthorResponse, CreateAuthorRequest, PaginatedRequest, UpdateAuthorRequest } from '../../../shared';
import { AuthorService } from './author-service';

@Injectable({
  providedIn: 'root'
})
export class AuthorServiceControllerService implements AuthorService {

  constructor(
    private readonly apiService: AuthorApiService,
    private readonly store: Store,
  ) { }

  getAuthorById(id: number): Observable<AuthorResponse> {
    return this.apiService.getById(id);
  }
  getAuthorsPaginated(request: PaginatedRequest): Observable<AuthorResponse[]> {
    this.store.dispatch(authorActions.getPaginated({ request: request }));
    return this.store.select(selectAuthors);
  }
  createAuthor(request: CreateAuthorRequest): void {
    this.store.dispatch(authorActions.create({ request: request }));
  }
  updateAuthor(request: UpdateAuthorRequest): void {
    this.store.dispatch(authorActions.update({ request: request }));
  }
  deleteAuthorById(id: number): void {
    this.store.dispatch(authorActions.deleteById({ id: id }));
  }
}