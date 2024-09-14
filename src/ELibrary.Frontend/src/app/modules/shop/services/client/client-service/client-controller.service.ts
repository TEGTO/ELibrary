import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ClientResponse, CreateClientRequest, UpdateClientRequest } from '../../../../shared';
import { ClientService } from './client-service';

@Injectable({
  providedIn: 'root'
})
export class ClientControllerService implements ClientService {
  getClient(): Observable<ClientResponse> {
    throw new Error('Method not implemented.');
  }
  createClient(req: CreateClientRequest): Observable<ClientResponse> {
    throw new Error('Method not implemented.');
  }
  updateClient(req: UpdateClientRequest): Observable<ClientResponse> {
    throw new Error('Method not implemented.');
  }
  deleteClient(): void {
    throw new Error('Method not implemented.');
  }

}
