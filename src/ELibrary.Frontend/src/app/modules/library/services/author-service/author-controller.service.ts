import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { authorActions, selectAuthorAmount, selectAuthors } from '../..';
import { AuthorApiService, AuthorResponse, CreateAuthorRequest, LibraryFilterRequest, UpdateAuthorRequest } from '../../../shared';
import { AuthorService } from './author-service';

@Injectable({
  providedIn: 'root'
})
export class AuthorControllerService implements AuthorService {

  constructor(
    private readonly apiService: AuthorApiService,
    private readonly store: Store,
  ) { }

  getById(id: number): Observable<AuthorResponse> {
    return this.apiService.getById(id);
  }
  getPaginated(request: LibraryFilterRequest): Observable<AuthorResponse[]> {
    this.store.dispatch(authorActions.getPaginated({ request: request }));
    return this.store.select(selectAuthors);
  }
  getItemTotalAmount(request: LibraryFilterRequest): Observable<number> {
    this.store.dispatch(authorActions.getTotalAmount({ request: request }));
    return this.store.select(selectAuthorAmount);
  }
  create(request: CreateAuthorRequest): void {
    this.store.dispatch(authorActions.create({ request: request }));
  }
  update(request: UpdateAuthorRequest): void {
    this.store.dispatch(authorActions.update({ request: request }));
  }
  deleteById(id: number): void {
    this.store.dispatch(authorActions.deleteById({ id: id }));
  }
}