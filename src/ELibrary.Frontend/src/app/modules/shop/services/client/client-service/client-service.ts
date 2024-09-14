import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ClientResponse, CreateClientRequest, UpdateClientRequest } from "../../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class ClientService {
    abstract getClient(): Observable<ClientResponse>;
    abstract createClient(req: CreateClientRequest): Observable<ClientResponse>;
    abstract updateClient(req: UpdateClientRequest): Observable<ClientResponse>;
    abstract deleteClient(): void;

}