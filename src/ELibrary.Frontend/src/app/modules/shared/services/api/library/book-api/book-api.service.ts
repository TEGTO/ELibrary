import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { BaseApiService, Book, BookFilterRequest, BookResponse, CreateBookRequest, LibraryEntityApi, mapBookResponseToBook, UpdateBookRequest } from '../../../..';

@Injectable({
  providedIn: 'root'
})
export class BookApiService extends BaseApiService implements LibraryEntityApi<Book, CreateBookRequest, UpdateBookRequest, BookFilterRequest> {
  getById(id: number): Observable<Book> {
    return this.httpClient.get<BookResponse>(this.combinePathWithBookApiUrl(`/${id}`)).pipe(
      map((response) => mapBookResponseToBook(response)),
      catchError((error) => this.handleError(error))
    );
  }
  getPaginated(request: BookFilterRequest): Observable<Book[]> {
    return this.httpClient.post<BookResponse[]>(this.combinePathWithBookApiUrl(`/pagination`), request).pipe(
      map(response => response.map(x => mapBookResponseToBook(x))),
      catchError((error) => this.handleError(error))
    );
  }
  getItemTotalAmount(request: BookFilterRequest): Observable<number> {
    return this.httpClient.post<number>(this.combinePathWithBookApiUrl(`/amount`), request).pipe(
      catchError((error) => this.handleError(error))
    );
  }
  create(request: CreateBookRequest): Observable<Book> {
    return this.httpClient.post<BookResponse>(this.combinePathWithBookApiUrl(``), request).pipe(
      map((response) => mapBookResponseToBook(response)),
      catchError((error) => this.handleError(error))
    );
  }
  update(request: UpdateBookRequest): Observable<Book> {
    return this.httpClient.put<BookResponse>(this.combinePathWithBookApiUrl(``), request).pipe(
      map((response) => mapBookResponseToBook(response)),
      catchError((error) => this.handleError(error))
    );
  }
  deleteById(id: number) {
    return this.httpClient.delete<void>(this.combinePathWithBookApiUrl(`/${id}`), { observe: 'response' }).pipe(
      catchError((error) => this.handleError(error))
    );
  }
  private combinePathWithBookApiUrl(subpath: string) {
    return this.urlDefiner.combineWithLibraryApiUrl("/book" + subpath);
  }
}