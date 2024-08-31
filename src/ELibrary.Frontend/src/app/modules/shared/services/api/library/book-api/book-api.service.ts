import { Injectable } from '@angular/core';
import { Observable, catchError } from 'rxjs';
import { BaseApiService, BookResponse, CreateBookRequest, PaginatedRequest, UpdateBookRequest } from '../../../..';

@Injectable({
  providedIn: 'root'
})
export class BookApiService extends BaseApiService {
  getById(id: number): Observable<BookResponse> {
    return this.httpClient.get<BookResponse>(this.combinePathWithBookApiUrl(`/${id}`)).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  getPaginated(request: PaginatedRequest): Observable<BookResponse[]> {
    return this.httpClient.post<BookResponse[]>(this.combinePathWithBookApiUrl(`/pagination`), request).pipe(
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