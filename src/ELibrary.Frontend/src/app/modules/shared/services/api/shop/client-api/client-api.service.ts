import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { Client, ClientResponse, CreateClientRequest, mapClientResponseToClient, UpdateClientRequest } from '../../../..';
import { BaseApiService } from '../../base-api/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class ClientApiService extends BaseApiService {
  get(): Observable<Client> {
    return this.httpClient.get<ClientResponse>(this.combinePathWithClientApiUrl(``)).pipe(
      map((response) => mapClientResponseToClient(response)),
      catchError((error) => this.handleError(error)),
    );
  }
  create(request: CreateClientRequest): Observable<Client> {
    return this.httpClient.post<ClientResponse>(this.combinePathWithClientApiUrl(``), request).pipe(
      map((response) => mapClientResponseToClient(response)),
      catchError((error) => this.handleError(error)),
    );
  }
  update(request: UpdateClientRequest): Observable<ClientResponse> {
    return this.httpClient.put<Client>(this.combinePathWithClientApiUrl(``), request).pipe(
      map((response) => mapClientResponseToClient(response)),
      catchError((error) => this.handleError(error)),
    );
  }
  delete(): Observable<HttpResponse<void>> {
    return this.httpClient.delete<void>(this.combinePathWithClientApiUrl(``), { observe: 'response' }).pipe(
      catchError((error) => this.handleError(error))
    );
  }

  private combinePathWithClientApiUrl(subpath: string): string {
    return this.urlDefiner.combineWithShopApiUrl("/client" + subpath);
  }
}
