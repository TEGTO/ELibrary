/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { deleteUser as deleteUserAuth, getAuthData as getUserAuth, logOutUser, refreshAccessToken, registerUser, selectAuthErrors, selectIsRefreshSuccessful, selectIsRegistrationSuccess, selectIsUpdateSuccess, selectUserAuth, signInUser, updateUserData } from '../..';
import { AuthToken, UserAuth, UserAuthenticationRequest, UserRegistrationRequest, UserUpdateRequest } from '../../../shared';
import { AuthenticationService } from './authentication-service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationControllerService implements AuthenticationService {

  constructor(
    private readonly store: Store,
  ) { }

  registerUser(req: UserRegistrationRequest): Observable<boolean> {
    this.store.dispatch(registerUser({ req: req }));
    return this.store.select(selectIsRegistrationSuccess);
  }
  getUserAuth(): Observable<UserAuth> {
    this.store.dispatch(getUserAuth());
    return this.store.select(selectUserAuth);
  }
  signInUser(req: UserAuthenticationRequest): void {
    this.store.dispatch(signInUser({ req: req }));
  }
  logOutUser(): void {
    this.store.dispatch(logOutUser());
  }
  refreshToken(authToken: AuthToken): Observable<boolean> {
    this.store.dispatch(refreshAccessToken({ authToken: authToken }));
    return this.store.select(selectIsRefreshSuccessful);
  }
  deleteUserAuth(): void {
    this.store.dispatch(deleteUserAuth());
  }
  updateUserAuth(req: UserUpdateRequest): Observable<boolean> {
    this.store.dispatch(updateUserData({ req: req }));
    return this.store.select(selectIsUpdateSuccess);
  }
  getAuthErrors(): Observable<any> {
    return this.store.select(selectAuthErrors);
  }
}