import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Client, CreateClientRequest, UpdateClientRequest } from "../../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class ClientService {
    abstract getClient(): Observable<Client>;
    abstract createClient(req: CreateClientRequest): Observable<Client>;
    abstract updateClient(req: UpdateClientRequest): Observable<Client>;
    abstract deleteClient(): void;

}