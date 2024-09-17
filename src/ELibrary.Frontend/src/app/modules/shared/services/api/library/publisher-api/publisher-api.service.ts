import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError } from 'rxjs';
import { BaseApiService, CreatePublisherRequest, LibraryEntityApi, LibraryFilterRequest, PublisherResponse, UpdatePublisherRequest } from '../../../..';

@Injectable({
  providedIn: 'root'
})
export class PublisherApiService extends BaseApiService implements LibraryEntityApi<PublisherResponse, CreatePublisherRequest, UpdatePublisherRequest, LibraryFilterRequest> {
  getById(id: number): Observable<PublisherResponse> {
    return this.httpClient.get<PublisherResponse>(this.combinePathWithPublisherApiUrl(`/${id}`)).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  getPaginated(request: LibraryFilterRequest): Observable<PublisherResponse[]> {
    return this.httpClient.post<PublisherResponse[]>(this.combinePathWithPublisherApiUrl(`/pagination`), request).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  getItemTotalAmount(request: LibraryFilterRequest): Observable<number> {
    return this.httpClient.post<number>(this.combinePathWithPublisherApiUrl(`/amount`), request).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  create(request: CreatePublisherRequest): Observable<PublisherResponse> {
    return this.httpClient.post<PublisherResponse>(this.combinePathWithPublisherApiUrl(``), request).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  update(request: UpdatePublisherRequest): Observable<PublisherResponse> {
    return this.httpClient.put<PublisherResponse>(this.combinePathWithPublisherApiUrl(``), request).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  deleteById(id: number): Observable<HttpResponse<void>> {
    return this.httpClient.delete<void>(this.combinePathWithPublisherApiUrl(`/${id}`), { observe: 'response' }).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  private combinePathWithPublisherApiUrl(subpath: string) {
    return this.urlDefiner.combineWithLibraryApiUrl("/publisher" + subpath);
  }
}