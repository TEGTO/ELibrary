import { Injectable } from '@angular/core';
import { Observable, catchError } from 'rxjs';
import { AuthToken, UserAuthenticationRequest, UserAuthenticationResponse, UserRegistrationRequest } from '../../..';
import { BaseApiService } from '../base-api/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationApiService extends BaseApiService {

  loginUser(userAuthData: UserAuthenticationRequest): Observable<UserAuthenticationResponse> {
    return this.httpClient.post<UserAuthenticationResponse>(this.combinePathWithAuthApiUrl(`/login`), userAuthData).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  registerUser(userRegistrationData: UserRegistrationRequest) {
    return this.httpClient.post(this.combinePathWithAuthApiUrl(`/register`), userRegistrationData).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  refreshToken(tokenData: AuthToken): Observable<AuthToken> {
    return this.httpClient.post<AuthToken>(this.combinePathWithAuthApiUrl(`/refresh`), tokenData).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  private combinePathWithAuthApiUrl(subpath: string) {
    return this.urlDefiner.combineWithUserApiUrl("/auth" + subpath);
  }
}