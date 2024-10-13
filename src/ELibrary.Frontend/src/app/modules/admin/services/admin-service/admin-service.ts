import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { AdminGetUserFilter, AdminUser, AdminUserRegistrationRequest, AdminUserUpdateDataRequest, Client, CreateClientRequest, UpdateClientRequest } from "../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class AdminService {
    abstract getUserById(id: string): Observable<AdminUser>;
    abstract registerUser(req: AdminUserRegistrationRequest): void;
    abstract getPaginatedUsers(req: AdminGetUserFilter): Observable<AdminUser[]>;
    abstract getPaginatedUserAmount(req: AdminGetUserFilter): Observable<number>;
    abstract updateUser(req: AdminUserUpdateDataRequest): void;
    abstract deleteUser(id: string): void;

    abstract getClientByUserId(id: string): Observable<Client | undefined>;
    abstract createClientForUser(userId: string, req: CreateClientRequest): void;
    abstract updateClientForUser(userId: string, req: UpdateClientRequest): void;
}