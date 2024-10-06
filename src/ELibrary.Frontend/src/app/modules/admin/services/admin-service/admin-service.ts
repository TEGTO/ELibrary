import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { AdminUser, AdminUserRegistrationRequest, AdminUserUpdateDataRequest, Client, CreateClientRequest, GetUserFilterRequest, UpdateClientRequest } from "../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class AdminService {
    abstract getUserById(id: string): Observable<AdminUser>;
    abstract registerUser(req: AdminUserRegistrationRequest): void;
    abstract getPaginatedUsers(req: GetUserFilterRequest): Observable<AdminUser[]>;
    abstract getPaginatedUserAmount(req: GetUserFilterRequest): Observable<number>;
    abstract updateUser(req: AdminUserUpdateDataRequest): void;
    abstract deleteUser(id: string): void;

    abstract getClientByUserId(id: string): Observable<Client | undefined>;
    abstract createClientForUser(userId: string, req: CreateClientRequest): void;
    abstract updateClientForUser(userId: string, req: UpdateClientRequest): void;
}