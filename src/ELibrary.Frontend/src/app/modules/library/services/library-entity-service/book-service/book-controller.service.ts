import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { bookActions, selectBookAmount, selectBooks } from '../../..';
import { Book, BookApiService, BookFilterRequest, CreateBookRequest, UpdateBookRequest } from '../../../../shared';
import { BaseControllerService } from '../base-entity-service/base-entity.service';
import { BookService } from './book-service';

@Injectable({
  providedIn: 'root'
})
export class BookControllerService extends BaseControllerService<
  Book,
  BookFilterRequest,
  CreateBookRequest,
  UpdateBookRequest
> implements BookService {
  constructor(
    apiService: BookApiService,
    store: Store
  ) {
    super(apiService, store, bookActions, {
      selectItems: selectBooks,
      selectTotalAmount: selectBookAmount
    });
  }
}