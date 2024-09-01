import { Injectable } from '@angular/core';
import { catchError, Observable } from 'rxjs';
import { BaseApiService, CreateGenreRequest, GenreResponse, LibraryEntityApi, PaginatedRequest, UpdateGenreRequest } from '../../../..';

@Injectable({
  providedIn: 'root'
})
export class GenreApiService extends BaseApiService implements LibraryEntityApi<GenreResponse, CreateGenreRequest, UpdateGenreRequest> {
  getById(id: number): Observable<GenreResponse> {
    return this.httpClient.get<GenreResponse>(this.combinePathWithGenreApiUrl(`/${id}`)).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  getPaginated(request: PaginatedRequest): Observable<GenreResponse[]> {
    return this.httpClient.post<GenreResponse[]>(this.combinePathWithGenreApiUrl(`/pagination`), request).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  getItemTotalAmount(): Observable<number> {
    return this.httpClient.get<number>(this.combinePathWithGenreApiUrl(`/amount`)).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  create(request: CreateGenreRequest): Observable<GenreResponse> {
    return this.httpClient.post<GenreResponse>(this.combinePathWithGenreApiUrl(``), request).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  update(request: UpdateGenreRequest) {
    return this.httpClient.put<GenreResponse>(this.combinePathWithGenreApiUrl(``), request).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  deleteById(id: number) {
    return this.httpClient.delete(this.combinePathWithGenreApiUrl(`/${id}`)).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  private combinePathWithGenreApiUrl(subpath: string) {
    return this.urlDefiner.combineWithLibraryApiUrl("/genre" + subpath);
  }
}