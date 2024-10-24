/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable, of } from 'rxjs';
import { deleteUser, getAuthData, logOutUser, oauthSignInUser, refreshAccessToken, registerUser, selectAuthErrors, selectIsRefreshSuccessful, selectIsRegistrationSuccess, selectIsUpdateSuccess, selectUserAuth, signInUser, updateUserData } from '../..';
import { AuthenticationApiService, AuthToken, GetOAuthUrl, GetOAuthUrlQueryParams, LocalStorageService, LoginOAuthRequest, UserAuth, UserAuthenticationRequest, UserRegistrationRequest, UserUpdateRequest } from '../../../shared';
import { AuthenticationService } from './authentication-service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationControllerService implements AuthenticationService {
  readonly storageOAuthParamsKey: string = "OAuthParams";

  constructor(
    private readonly store: Store,
    private readonly authApiService: AuthenticationApiService,
    private readonly localStorage: LocalStorageService
  ) { }

  registerUser(req: UserRegistrationRequest): Observable<boolean> {
    this.store.dispatch(registerUser({ req: req }));
    return this.store.select(selectIsRegistrationSuccess);
  }
  getUserAuth(): Observable<UserAuth> {
    this.store.dispatch(getAuthData());
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
    this.store.dispatch(deleteUser());
  }
  updateUserAuth(req: UserUpdateRequest): Observable<boolean> {
    this.store.dispatch(updateUserData({ req: req }));
    return this.store.select(selectIsUpdateSuccess);
  }
  getAuthErrors(): Observable<any> {
    return this.store.select(selectAuthErrors);
  }
  getOAuthUrl(req: GetOAuthUrlQueryParams): Observable<GetOAuthUrl> {
    return this.authApiService.getOAuthUrl(req);
  }
  oauthSignIn(req: LoginOAuthRequest): void {
    this.store.dispatch(oauthSignInUser({ req: req }));
  }
  setOAuthParams(oAuthParams: GetOAuthUrlQueryParams): void {
    this.localStorage.setItem(this.storageOAuthParamsKey, JSON.stringify(oAuthParams));
  }
  getOAuthParams(): Observable<GetOAuthUrlQueryParams | null> {
    const json = this.localStorage.getItem(this.storageOAuthParamsKey);
    if (json !== null) {
      const oauthParams: GetOAuthUrlQueryParams = JSON.parse(json);
      return of(oauthParams);
    }
    return of(null);
  }
}