import { Injectable } from '@angular/core';
import { catchError, map, Observable, of } from 'rxjs';
import { Client, ClientResponse, CreateClientRequest, mapClientResponseToClient, UpdateClientRequest } from '../../../..';
import { BaseApiService } from '../../base-api/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class ClientApiService extends BaseApiService {

  //#region User

  get(): Observable<Client> {
    return this.httpClient.get<ClientResponse>(this.combinePathWithClientApiUrl(``)).pipe(
      map((response) => mapClientResponseToClient(response)),
      catchError((error) => this.handleNotFoundError(error)),
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

  //#endregion

  //#region Admin

  adminGet(id: string): Observable<Client> {
    return this.httpClient.get<ClientResponse>(this.combinePathWithClientApiUrl(`admin/${id}`)).pipe(
      map((response) => mapClientResponseToClient(response)),
      catchError((error) => this.handleError(error)),
    );
  }
  adminCreate(id: string, request: CreateClientRequest): Observable<Client> {
    return this.httpClient.post<ClientResponse>(this.combinePathWithClientApiUrl(`admin/${id}`), request).pipe(
      map((response) => mapClientResponseToClient(response)),
      catchError((error) => this.handleError(error)),
    );
  }
  adminUpdate(id: string, request: UpdateClientRequest): Observable<ClientResponse> {
    return this.httpClient.put<Client>(this.combinePathWithClientApiUrl(`admin/${id}`), request).pipe(
      map((response) => mapClientResponseToClient(response)),
      catchError((error) => this.handleError(error)),
    );
  }

  //#endregion

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  private handleNotFoundError(error: any): Observable<never> {
    if ('status' in error && error.status === 404) {
      return of();
    }
    return this.handleError(error);
  }
  private combinePathWithClientApiUrl(subpath: string): string {
    return this.urlDefiner.combineWithShopApiUrl("/client" + subpath);
  }
}
