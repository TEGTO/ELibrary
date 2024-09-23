import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { createClient, deleteClient, getClient, selectClient, updateClient } from '../..';
import { Client, CreateClientRequest, UpdateClientRequest } from '../../../shared';
import { ClientService } from './client-service';

@Injectable({
  providedIn: 'root'
})
export class ClientControllerService implements ClientService {

  constructor(
    private readonly store: Store,
  ) { }

  getClient(): Observable<Client | null> {
    this.store.dispatch(getClient());
    return this.store.select(selectClient);
  }
  createClient(req: CreateClientRequest): void {
    this.store.dispatch(createClient({ req: req }));
  }
  updateClient(req: UpdateClientRequest): void {
    this.store.dispatch(updateClient({ req: req }));
  }
  deleteClient(): void {
    this.store.dispatch(deleteClient());
  }
}
