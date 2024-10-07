import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { createClient, deleteUser, getClient, getPaginatedUserAmount, getPaginatedUsers, getUser, registerUser, selectClientByUserId, selectUserById, selectUsers, selectUserTotalAmount, updateClient, updateUser } from '../..';
import { AdminGetUserFilter, AdminUser, AdminUserRegistrationRequest, AdminUserUpdateDataRequest, Client, CreateClientRequest, UpdateClientRequest } from '../../../shared';
import { AdminService } from './admin-service';

@Injectable({
  providedIn: 'root'
})
export class AdminControllerService implements AdminService {

  constructor(
    private readonly store: Store,
  ) { }

  getUserById(id: string): Observable<AdminUser> {
    this.store.dispatch(getUser({ info: id }));
    return this.store.select(selectUserById(id));
  }
  registerUser(req: AdminUserRegistrationRequest): void {
    this.store.dispatch(registerUser({ req: req }));
  }
  getPaginatedUsers(req: AdminGetUserFilter): Observable<AdminUser[]> {
    this.store.dispatch(getPaginatedUsers({ req: req }));
    return this.store.select(selectUsers);
  }
  getPaginatedUserAmount(req: AdminGetUserFilter): Observable<number> {
    this.store.dispatch(getPaginatedUserAmount({ req: req }));
    return this.store.select(selectUserTotalAmount);
  }
  updateUser(req: AdminUserUpdateDataRequest): void {
    this.store.dispatch(updateUser({ req: req }));
  }
  deleteUser(id: string): void {
    this.store.dispatch(deleteUser({ id: id }));
  }
  getClientByUserId(userId: string): Observable<Client | undefined> {
    this.store.dispatch(getClient({ userId: userId }));
    return this.store.select(selectClientByUserId(userId));
  }
  createClientForUser(userId: string, req: CreateClientRequest): void {
    this.store.dispatch(createClient({ userId: userId, req: req }));
  }
  updateClientForUser(userId: string, req: UpdateClientRequest): void {
    this.store.dispatch(updateClient({ userId: userId, req: req }));
  }
} 
