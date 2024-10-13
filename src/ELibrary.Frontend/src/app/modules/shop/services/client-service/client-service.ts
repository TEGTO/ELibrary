import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Client, CreateClientRequest, UpdateClientRequest } from "../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class ClientService {
    abstract getClient(): Observable<Client | null>;
    abstract createClient(req: CreateClientRequest): void;
    abstract updateClient(req: UpdateClientRequest): void;
}