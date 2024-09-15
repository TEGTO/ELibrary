import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { AuthorResponse, BaseApiService, CreateAuthorRequest, LibraryEntityApi, LibraryFilterRequest, mapAuthorData, UpdateAuthorRequest } from '../../../..';

@Injectable({
  providedIn: 'root'
})
export class AuthorApiService extends BaseApiService implements LibraryEntityApi<AuthorResponse, CreateAuthorRequest, UpdateAuthorRequest, LibraryFilterRequest> {
  getById(id: number): Observable<AuthorResponse> {
    return this.httpClient.get<AuthorResponse>(this.combinePathWithAuthorApiUrl(`/${id}`)).pipe(
      map(resp => mapAuthorData(resp)),
      catchError((resp) => this.handleError(resp))
    );
  }
  getPaginated(request: LibraryFilterRequest): Observable<AuthorResponse[]> {
    return this.httpClient.post<AuthorResponse[]>(this.combinePathWithAuthorApiUrl(`/pagination`), request).pipe(
      map(resp => resp.map(x => mapAuthorData(x))),
      catchError((resp) => this.handleError(resp))
    );
  }
  getItemTotalAmount(request: LibraryFilterRequest): Observable<number> {
    return this.httpClient.post<number>(this.combinePathWithAuthorApiUrl(`/amount`), request).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  create(request: CreateAuthorRequest): Observable<AuthorResponse> {
    return this.httpClient.post<AuthorResponse>(this.combinePathWithAuthorApiUrl(``), request).pipe(
      map(resp => mapAuthorData(resp)),
      catchError((resp) => this.handleError(resp))
    );
  }
  update(request: UpdateAuthorRequest): Observable<AuthorResponse> {
    return this.httpClient.put<AuthorResponse>(this.combinePathWithAuthorApiUrl(``), request).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  deleteById(id: number): Observable<HttpResponse<void>> {
    return this.httpClient.delete<void>(this.combinePathWithAuthorApiUrl(`/${id}`), { observe: 'response' }).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  private combinePathWithAuthorApiUrl(subpath: string) {
    return this.urlDefiner.combineWithLibraryApiUrl("/author" + subpath);
  }
}