import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { AuthorResponse, BaseApiService, CreateAuthorRequest, mapAuthorResponseData, PaginatedRequest, UpdateAuthorRequest } from '../../../..';

@Injectable({
  providedIn: 'root'
})
export class AuthorApiService extends BaseApiService {
  getById(id: number): Observable<AuthorResponse> {
    return this.httpClient.get<AuthorResponse>(this.combinePathWithAuthorApiUrl(`/${id}`)).pipe(
      map(resp => mapAuthorResponseData(resp)),
      catchError((resp) => this.handleError(resp))
    );
  }
  getPaginated(request: PaginatedRequest): Observable<AuthorResponse[]> {
    return this.httpClient.post<AuthorResponse[]>(this.combinePathWithAuthorApiUrl(`/pagination`), request).pipe(
      map(resp => resp.map(x => mapAuthorResponseData(x))),
      catchError((resp) => this.handleError(resp))
    );
  }
  create(request: CreateAuthorRequest): Observable<AuthorResponse> {
    return this.httpClient.post<AuthorResponse>(this.combinePathWithAuthorApiUrl(``), request).pipe(
      map(resp => mapAuthorResponseData(resp)),
      catchError((resp) => this.handleError(resp))
    );
  }
  update(request: UpdateAuthorRequest) {
    return this.httpClient.put(this.combinePathWithAuthorApiUrl(``), request).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  deleteById(id: number) {
    return this.httpClient.delete(this.combinePathWithAuthorApiUrl(`/${id}`)).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  private combinePathWithAuthorApiUrl(subpath: string) {
    return this.urlDefiner.combineWithLibraryApiUrl("/author" + subpath);
  }
}