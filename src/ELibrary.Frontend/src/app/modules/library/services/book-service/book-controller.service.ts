import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { bookActions, selectBooks } from '../..';
import { BookApiService, BookResponse, CreateBookRequest, PaginatedRequest, UpdateBookRequest } from '../../../shared';
import { BookService } from './book-service';

@Injectable({
  providedIn: 'root'
})
export class BookServiceControllerService implements BookService {

  constructor(
    private readonly apiService: BookApiService,
    private readonly store: Store,
  ) { }

  getBookById(id: number): Observable<BookResponse> {
    return this.apiService.getById(id);
  }
  getBooksPaginated(request: PaginatedRequest): Observable<BookResponse[]> {
    this.store.dispatch(bookActions.getPaginated({ request }));
    return this.store.select(selectBooks);
  }
  createBook(request: CreateBookRequest): void {
    this.store.dispatch(bookActions.create({ request: request }));
  }
  updateBook(request: UpdateBookRequest): void {
    this.store.dispatch(bookActions.update({ request: request }));
  }
  deleteBookById(id: number): void {
    this.store.dispatch(bookActions.deleteById({ id: id }));
  }
}