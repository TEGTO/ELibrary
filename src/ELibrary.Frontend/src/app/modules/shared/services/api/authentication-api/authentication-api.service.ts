import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { AdminUser, AdminUserRegistrationRequest, AdminUserResponse, AuthToken, AuthTokenResponse, GetOAuthUrl, GetOAuthUrlQueryParams, GetOAuthUrlResponse, LoginOAuthRequest, mapAdminUserResponseToAdminUser, mapAuthTokenResponseToAuthToken, mapGetOAuthUrlResponseToGetOAuthUrl, mapUserAuthenticationResponseToUserAuthentication, UserAuth, UserAuthenticationRequest, UserAuthenticationResponse, UserRegistrationRequest } from '../../..';
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
  getOAuthUrl(req: GetOAuthUrlQueryParams): Observable<GetOAuthUrl> {
    const params = new HttpParams()
      .set('OAuthLoginProvider', req.oAuthLoginProvider)
      .set('redirectUrl', req.redirectUrl)
      .set('codeVerifier', req.codeVerifier);
    return this.httpClient.get<GetOAuthUrlResponse>(
      this.combinePathWithAuthApiUrl(`/oauth`), { params }
    ).pipe(
      map((response) => mapGetOAuthUrlResponseToGetOAuthUrl(response)),
      catchError((resp) => this.handleError(resp))
    );
  }
  loginUserOAuth(req: LoginOAuthRequest): Observable<UserAuth> {
    return this.httpClient.post<UserAuthenticationResponse>(this.combinePathWithAuthApiUrl(`/oauth`), req).pipe(
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

  //#endregion

  //#region Admin

  adminRegisterUser(req: AdminUserRegistrationRequest): Observable<AdminUser> {
    return this.httpClient.post<AdminUserResponse>(this.combinePathWithAuthApiUrl(`/admin/register`), req).pipe(
      map((response) => mapAdminUserResponseToAdminUser(response)),
      catchError((resp) => this.handleError(resp))
    );
  }

  //#endregion

  private combinePathWithAuthApiUrl(subpath: string): string {
    return this.urlDefiner.combineWithUserApiUrl("/auth" + subpath);
  }
}