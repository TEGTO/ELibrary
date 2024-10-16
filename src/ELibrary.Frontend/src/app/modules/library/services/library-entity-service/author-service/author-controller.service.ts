import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { authorActions, selectAuthorAmount, selectAuthors } from '../../..';
import { Author, AuthorApiService, CreateAuthorRequest, LibraryFilterRequest, UpdateAuthorRequest } from '../../../../shared';
import { BaseControllerService } from '../base-entity-service/base-entity.service';
import { AuthorService } from './author-service';

@Injectable({
  providedIn: 'root'
})
export class AuthorControllerService extends BaseControllerService<
  Author,
  LibraryFilterRequest,
  CreateAuthorRequest,
  UpdateAuthorRequest
> implements AuthorService {
  constructor(
    apiService: AuthorApiService,
    store: Store
  ) {
    super(apiService, store, authorActions, {
      selectItems: selectAuthors,
      selectTotalAmount: selectAuthorAmount
    });
  }
}