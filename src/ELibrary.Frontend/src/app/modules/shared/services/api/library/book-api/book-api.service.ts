import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { BaseApiService, BookFilterRequest, BookResponse, CreateBookRequest, LibraryEntityApi, mapBookData, UpdateBookRequest } from '../../../..';

@Injectable({
  providedIn: 'root'
})
export class BookApiService extends BaseApiService implements LibraryEntityApi<BookResponse, CreateBookRequest, UpdateBookRequest, BookFilterRequest> {
  getById(id: number): Observable<BookResponse> {
    return this.httpClient.get<BookResponse>(this.combinePathWithBookApiUrl(`/${id}`)).pipe(
      map(resp => mapBookData(resp)),
      catchError((resp) => this.handleError(resp))
    );
  }
  getPaginated(request: BookFilterRequest): Observable<BookResponse[]> {
    return this.httpClient.post<BookResponse[]>(this.combinePathWithBookApiUrl(`/pagination`), request).pipe(
      map(resp => resp.map(x => mapBookData(x))),
      catchError((resp) => this.handleError(resp))
    );
  }
  getItemTotalAmount(request: BookFilterRequest): Observable<number> {
    return this.httpClient.post<number>(this.combinePathWithBookApiUrl(`/amount`), request).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  create(request: CreateBookRequest): Observable<BookResponse> {
    return this.httpClient.post<BookResponse>(this.combinePathWithBookApiUrl(``), request).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  update(request: UpdateBookRequest): Observable<BookResponse> {
    return this.httpClient.put<BookResponse>(this.combinePathWithBookApiUrl(``), request).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  deleteById(id: number) {
    return this.httpClient.delete<void>(this.combinePathWithBookApiUrl(`/${id}`), { observe: 'response' }).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  private combinePathWithBookApiUrl(subpath: string) {
    return this.urlDefiner.combineWithLibraryApiUrl("/book" + subpath);
  }
}