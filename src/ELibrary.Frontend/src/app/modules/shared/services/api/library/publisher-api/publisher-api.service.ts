import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { BaseApiService, CreatePublisherRequest, LibraryEntityApi, LibraryFilterRequest, mapPublisherResponseToPublisher, Publisher, PublisherResponse, UpdatePublisherRequest } from '../../../..';

@Injectable({
  providedIn: 'root'
})
export class PublisherApiService extends BaseApiService implements LibraryEntityApi<Publisher, CreatePublisherRequest, UpdatePublisherRequest, LibraryFilterRequest> {
  getById(id: number): Observable<Publisher> {
    return this.httpClient.get<PublisherResponse>(this.combinePathWithPublisherApiUrl(`/${id}`)).pipe(
      map((response) => mapPublisherResponseToPublisher(response)),
      catchError((error) => this.handleError(error))
    );
  }
  getPaginated(request: LibraryFilterRequest): Observable<Publisher[]> {
    return this.httpClient.post<PublisherResponse[]>(this.combinePathWithPublisherApiUrl(`/pagination`), request).pipe(
      map((response) => response.map(x => mapPublisherResponseToPublisher(x))),
      catchError((error) => this.handleError(error))
    );
  }
  getItemTotalAmount(request: LibraryFilterRequest): Observable<number> {
    return this.httpClient.post<number>(this.combinePathWithPublisherApiUrl(`/amount`), request).pipe(
      catchError((error) => this.handleError(error))
    );
  }
  create(request: CreatePublisherRequest): Observable<Publisher> {
    return this.httpClient.post<PublisherResponse>(this.combinePathWithPublisherApiUrl(``), request).pipe(
      map((response) => mapPublisherResponseToPublisher(response)),
      catchError((error) => this.handleError(error))
    );
  }
  update(request: UpdatePublisherRequest): Observable<Publisher> {
    return this.httpClient.put<PublisherResponse>(this.combinePathWithPublisherApiUrl(``), request).pipe(
      map((response) => mapPublisherResponseToPublisher(response)),
      catchError((error) => this.handleError(error))
    );
  }
  deleteById(id: number): Observable<HttpResponse<void>> {
    return this.httpClient.delete<void>(this.combinePathWithPublisherApiUrl(`/${id}`), { observe: 'response' }).pipe(
      catchError((error) => this.handleError(error))
    );
  }
  private combinePathWithPublisherApiUrl(subpath: string) {
    return this.urlDefiner.combineWithLibraryApiUrl("/publisher" + subpath);
  }
}