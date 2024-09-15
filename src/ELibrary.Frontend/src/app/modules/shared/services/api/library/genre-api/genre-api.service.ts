import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable } from 'rxjs';
import { BaseApiService, CreateGenreRequest, GenreResponse, LibraryEntityApi, LibraryFilterRequest, UpdateGenreRequest } from '../../../..';

@Injectable({
  providedIn: 'root'
})
export class GenreApiService extends BaseApiService implements LibraryEntityApi<GenreResponse, CreateGenreRequest, UpdateGenreRequest, LibraryFilterRequest> {
  getById(id: number): Observable<GenreResponse> {
    return this.httpClient.get<GenreResponse>(this.combinePathWithGenreApiUrl(`/${id}`)).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  getPaginated(request: LibraryFilterRequest): Observable<GenreResponse[]> {
    return this.httpClient.post<GenreResponse[]>(this.combinePathWithGenreApiUrl(`/pagination`), request).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  getItemTotalAmount(request: LibraryFilterRequest): Observable<number> {
    return this.httpClient.post<number>(this.combinePathWithGenreApiUrl(`/amount`), request).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  create(request: CreateGenreRequest): Observable<GenreResponse> {
    return this.httpClient.post<GenreResponse>(this.combinePathWithGenreApiUrl(``), request).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  update(request: UpdateGenreRequest): Observable<GenreResponse> {
    return this.httpClient.put<GenreResponse>(this.combinePathWithGenreApiUrl(``), request).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  deleteById(id: number): Observable<HttpResponse<void>> {
    return this.httpClient.delete<void>(this.combinePathWithGenreApiUrl(`/${id}`), { observe: 'response' }).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  private combinePathWithGenreApiUrl(subpath: string) {
    return this.urlDefiner.combineWithLibraryApiUrl("/genre" + subpath);
  }
}