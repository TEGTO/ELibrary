import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { bookActions, selectBookAmount, selectBooks } from '../..';
import { BookApiService, BookFilterRequest, BookResponse, CreateBookRequest, UpdateBookRequest } from '../../../shared';
import { BookService } from './book-service';

@Injectable({
  providedIn: 'root'
})
export class BookControllerService implements BookService {

  constructor(
    private readonly apiService: BookApiService,
    private readonly store: Store,
  ) { }

  getById(id: number): Observable<BookResponse> {
    return this.apiService.getById(id);
  }
  getPaginated(request: BookFilterRequest): Observable<BookResponse[]> {
    this.store.dispatch(bookActions.getPaginated({ request: request }));
    return this.store.select(selectBooks);
  }
  getItemTotalAmount(request: BookFilterRequest): Observable<number> {
    this.store.dispatch(bookActions.getTotalAmount({ request: request }));
    return this.store.select(selectBookAmount);
  }
  create(request: CreateBookRequest): void {
    this.store.dispatch(bookActions.create({ request: request }));
  }
  update(request: UpdateBookRequest): void {
    this.store.dispatch(bookActions.update({ request: request }));
  }
  deleteById(id: number): void {
    this.store.dispatch(bookActions.deleteById({ id: id }));
  }
}