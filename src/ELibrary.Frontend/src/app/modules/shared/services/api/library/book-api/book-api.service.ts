import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { BaseApiService, BookResponse, CreateBookRequest, LibraryEntityApi, mapBookData, PaginatedRequest, UpdateBookRequest } from '../../../..';

@Injectable({
  providedIn: 'root'
})
export class BookApiService extends BaseApiService implements LibraryEntityApi<BookResponse, CreateBookRequest, UpdateBookRequest> {
  getById(id: number): Observable<BookResponse> {
    return this.httpClient.get<BookResponse>(this.combinePathWithBookApiUrl(`/${id}`)).pipe(
      map(resp => mapBookData(resp)),
      catchError((resp) => this.handleError(resp))
    );
  }
  getPaginated(request: PaginatedRequest): Observable<BookResponse[]> {
    return this.httpClient.post<BookResponse[]>(this.combinePathWithBookApiUrl(`/pagination`), request).pipe(
      map(resp => resp.map(x => mapBookData(x))),
      catchError((resp) => this.handleError(resp))
    );
  }
  getItemTotalAmount(): Observable<number> {
    return this.httpClient.get<number>(this.combinePathWithBookApiUrl(`/amount`)).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  create(request: CreateBookRequest): Observable<BookResponse> {
    return this.httpClient.post<BookResponse>(this.combinePathWithBookApiUrl(``), request).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  update(request: UpdateBookRequest) {
    return this.httpClient.put(this.combinePathWithBookApiUrl(``), request).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  deleteById(id: number) {
    return this.httpClient.delete(this.combinePathWithBookApiUrl(`/${id}`)).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  private combinePathWithBookApiUrl(subpath: string) {
    return this.urlDefiner.combineWithLibraryApiUrl("/book" + subpath);
  }
}