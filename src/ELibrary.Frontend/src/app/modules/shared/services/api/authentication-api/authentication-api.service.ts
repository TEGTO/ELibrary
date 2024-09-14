import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError } from 'rxjs';
import { AuthToken, UserAuthenticationRequest, UserAuthenticationResponse, UserRegistrationRequest, UserUpdateRequest } from '../../..';
import { BaseApiService } from '../base-api/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationApiService extends BaseApiService {

  loginUser(req: UserAuthenticationRequest): Observable<UserAuthenticationResponse> {
    return this.httpClient.post<UserAuthenticationResponse>(this.combinePathWithAuthApiUrl(`/login`), req).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }

  registerUser(req: UserRegistrationRequest): Observable<UserAuthenticationResponse> {
    return this.httpClient.post<UserAuthenticationResponse>(this.combinePathWithAuthApiUrl(`/register`), req).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }

  refreshToken(tokenData: AuthToken): Observable<AuthToken> {
    return this.httpClient.post<AuthToken>(this.combinePathWithAuthApiUrl(`/refresh`), tokenData).pipe(
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

  private combinePathWithAuthApiUrl(subpath: string): string {
    return this.urlDefiner.combineWithUserApiUrl("/user" + subpath);
  }
}