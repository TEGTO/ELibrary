import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError } from 'rxjs';
import { ClientResponse, CreateClientRequest, UpdateClientRequest } from '../../../..';
import { BaseApiService } from '../../base-api/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class ClientApiService extends BaseApiService {
  get(): Observable<ClientResponse> {
    return this.httpClient.get<ClientResponse>(this.combinePathWithClientApiUrl(``)).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  create(request: CreateClientRequest): Observable<ClientResponse> {
    return this.httpClient.post<ClientResponse>(this.combinePathWithClientApiUrl(``), request).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  update(request: UpdateClientRequest): Observable<ClientResponse> {
    return this.httpClient.put<ClientResponse>(this.combinePathWithClientApiUrl(``), request).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  delete(): Observable<HttpResponse<void>> {
    return this.httpClient.delete<void>(this.combinePathWithClientApiUrl(``), { observe: 'response' }).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }

  private combinePathWithClientApiUrl(subpath: string): string {
    return this.urlDefiner.combineWithShopApiUrl("/client" + subpath);
  }
}
