import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { AdminGetUserFilter, AdminUser, AdminUserRegistrationRequest, AdminUserResponse, AdminUserUpdateDataRequest, AuthToken, AuthTokenResponse, mapAdminUserResponseToAdminUser, mapAuthTokenResponseToAuthToken, mapUserAuthenticationResponseToUserAuthentication, UserAuth, UserAuthenticationRequest, UserAuthenticationResponse, UserRegistrationRequest, UserUpdateRequest } from '../../..';
import { BaseApiService } from '../base-api/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationApiService extends BaseApiService {

  //#region User

  loginUser(req: UserAuthenticationRequest): Observable<UserAuth> {
    return this.httpClient.post<UserAuthenticationResponse>(this.combinePathWithAuthApiUrl(`/login`), req).pipe(
      map((response) => mapUserAuthenticationResponseToUserAuthentication(response)),
      catchError((resp) => this.handleError(resp))
    );
  }
  registerUser(req: UserRegistrationRequest): Observable<UserAuth> {
    return this.httpClient.post<UserAuthenticationResponse>(this.combinePathWithAuthApiUrl(`/register`), req).pipe(
      map((response) => mapUserAuthenticationResponseToUserAuthentication(response)),
      catchError((resp) => this.handleError(resp))
    );
  }
  refreshToken(tokenData: AuthToken): Observable<AuthToken> {
    return this.httpClient.post<AuthTokenResponse>(this.combinePathWithAuthApiUrl(`/refresh`), tokenData).pipe(
      map((response) => mapAuthTokenResponseToAuthToken(response)),
      catchError((resp) => this.handleError(resp))
    );
  }
  updateUser(req: UserUpdateRequest): Observable<HttpResponse<void>> {
    return this.httpClient.put<void>(this.combinePathWithAuthApiUrl(`/update`), req, { observe: 'response' }).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  deleteUser(): Observable<HttpResponse<void>> {
    return this.httpClient.delete<void>(this.combinePathWithAuthApiUrl(`/delete`), { observe: 'response' }).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }

  //#endregion

  //#region Admin

  adminRegisterUser(req: AdminUserRegistrationRequest): Observable<AdminUser> {
    return this.httpClient.post<AdminUserResponse>(this.combinePathWithAuthApiUrl(`/admin/register`), req).pipe(
      map((response) => mapAdminUserResponseToAdminUser(response)),
      catchError((resp) => this.handleError(resp))
    );
  }
  adminGetUser(info: string): Observable<AdminUser> {
    return this.httpClient.get<AdminUserResponse>(this.combinePathWithAuthApiUrl(`/admin/users/${info}`)).pipe(
      map((response) => mapAdminUserResponseToAdminUser(response)),
      catchError((resp) => this.handleError(resp))
    );
  }
  adminGetPaginatedUsers(req: AdminGetUserFilter): Observable<AdminUser[]> {
    return this.httpClient.post<AdminUserResponse[]>(this.combinePathWithAuthApiUrl(`/admin/users`), req).pipe(
      map((response) => response.map(x => mapAdminUserResponseToAdminUser(x))),
      catchError((resp) => this.handleError(resp))
    );
  }
  adminGetPaginatedUserAmount(req: AdminGetUserFilter): Observable<number> {
    return this.httpClient.post<number>(this.combinePathWithAuthApiUrl(`/admin/users/amount`), req).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  adminUpdateUser(req: AdminUserUpdateDataRequest): Observable<AdminUser> {
    return this.httpClient.put<AdminUserResponse>(this.combinePathWithAuthApiUrl(`/admin/update`), req).pipe(
      map((response) => mapAdminUserResponseToAdminUser(response)),
      catchError((resp) => this.handleError(resp))
    );
  }
  adminDeleteUser(id: string): Observable<HttpResponse<void>> {
    return this.httpClient.delete<void>(this.combinePathWithAuthApiUrl(`/admin/delete/${id}`), { observe: 'response' }).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }

  //#endregion

  private combinePathWithAuthApiUrl(subpath: string): string {
    return this.urlDefiner.combineWithUserApiUrl("/user" + subpath);
  }
}