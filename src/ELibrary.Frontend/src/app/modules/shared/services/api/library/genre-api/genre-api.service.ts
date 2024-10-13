import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { BaseApiService, CreateGenreRequest, Genre, GenreResponse, LibraryEntityApi, LibraryFilterRequest, mapGenreResponseToGenre, UpdateGenreRequest } from '../../../..';

@Injectable({
  providedIn: 'root'
})
export class GenreApiService extends BaseApiService implements LibraryEntityApi<Genre, CreateGenreRequest, UpdateGenreRequest, LibraryFilterRequest> {
  getById(id: number): Observable<Genre> {
    return this.httpClient.get<GenreResponse>(this.combinePathWithGenreApiUrl(`/${id}`)).pipe(
      map((response) => mapGenreResponseToGenre(response)),
      catchError((error) => this.handleError(error))
    );
  }
  getPaginated(request: LibraryFilterRequest): Observable<Genre[]> {
    return this.httpClient.post<GenreResponse[]>(this.combinePathWithGenreApiUrl(`/pagination`), request).pipe(
      map((response) => response.map(x => mapGenreResponseToGenre(x))),
      catchError((error) => this.handleError(error))
    );
  }
  getItemTotalAmount(request: LibraryFilterRequest): Observable<number> {
    return this.httpClient.post<number>(this.combinePathWithGenreApiUrl(`/amount`), request).pipe(
      catchError((error) => this.handleError(error))
    );
  }
  create(request: CreateGenreRequest): Observable<Genre> {
    return this.httpClient.post<GenreResponse>(this.combinePathWithGenreApiUrl(``), request).pipe(
      map((response) => mapGenreResponseToGenre(response)),
      catchError((error) => this.handleError(error))
    );
  }
  update(request: UpdateGenreRequest): Observable<Genre> {
    return this.httpClient.put<GenreResponse>(this.combinePathWithGenreApiUrl(``), request).pipe(
      map((response) => mapGenreResponseToGenre(response)),
      catchError((error) => this.handleError(error))
    );
  }
  deleteById(id: number): Observable<HttpResponse<void>> {
    return this.httpClient.delete<void>(this.combinePathWithGenreApiUrl(`/${id}`), { observe: 'response' }).pipe(
      catchError((error) => this.handleError(error))
    );
  }
  private combinePathWithGenreApiUrl(subpath: string) {
    return this.urlDefiner.combineWithLibraryApiUrl("/genre" + subpath);
  }
}