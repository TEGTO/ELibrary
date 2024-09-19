import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { Author, AuthorResponse, BaseApiService, CreateAuthorRequest, LibraryEntityApi, LibraryFilterRequest, mapAuthorResponseToAuthor, UpdateAuthorRequest } from '../../../..';

@Injectable({
  providedIn: 'root'
})
export class AuthorApiService extends BaseApiService implements LibraryEntityApi<Author, CreateAuthorRequest, UpdateAuthorRequest, LibraryFilterRequest> {
  getById(id: number): Observable<Author> {
    return this.httpClient.get<AuthorResponse>(this.combinePathWithAuthorApiUrl(`/${id}`)).pipe(
      map((response) => mapAuthorResponseToAuthor(response)),
      catchError((error) => this.handleError(error))
    );
  }
  getPaginated(request: LibraryFilterRequest): Observable<Author[]> {
    return this.httpClient.post<AuthorResponse[]>(this.combinePathWithAuthorApiUrl(`/pagination`), request).pipe(
      map((response) => response.map(x => mapAuthorResponseToAuthor(x))),
      catchError((error) => this.handleError(error))
    );
  }
  getItemTotalAmount(request: LibraryFilterRequest): Observable<number> {
    return this.httpClient.post<number>(this.combinePathWithAuthorApiUrl(`/amount`), request).pipe(
      catchError((error) => this.handleError(error))
    );
  }
  create(request: CreateAuthorRequest): Observable<Author> {
    return this.httpClient.post<AuthorResponse>(this.combinePathWithAuthorApiUrl(``), request).pipe(
      map((response) => mapAuthorResponseToAuthor(response)),
      catchError((error) => this.handleError(error))
    );
  }
  update(request: UpdateAuthorRequest): Observable<Author> {
    return this.httpClient.put<AuthorResponse>(this.combinePathWithAuthorApiUrl(``), request).pipe(
      map((response) => mapAuthorResponseToAuthor(response)),
      catchError((error) => this.handleError(error))
    );
  }
  deleteById(id: number): Observable<HttpResponse<void>> {
    return this.httpClient.delete<void>(this.combinePathWithAuthorApiUrl(`/${id}`), { observe: 'response' }).pipe(
      catchError((error) => this.handleError(error))
    );
  }
  private combinePathWithAuthorApiUrl(subpath: string) {
    return this.urlDefiner.combineWithLibraryApiUrl("/author" + subpath);
  }
}