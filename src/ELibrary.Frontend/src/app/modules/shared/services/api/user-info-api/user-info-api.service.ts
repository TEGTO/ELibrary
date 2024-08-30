import { Injectable } from '@angular/core';
import { Observable, catchError, map } from 'rxjs';
import { BaseApiService, GetCurrentUserResponse, mapGetCurrentUserResponseData } from '../../..';

@Injectable({
  providedIn: 'root'
})
export class UserInfoApiService extends BaseApiService {

  getCurrentUser(): Observable<GetCurrentUserResponse> {
    return this.httpClient.get<GetCurrentUserResponse>(this.combinePathWithUserInfoApiUrl(`/user`)).pipe(
      map(resp => mapGetCurrentUserResponseData(resp)),
      catchError((resp) => this.handleError(resp))
    );
  }
  private combinePathWithUserInfoApiUrl(subpath: string) {
    return this.urlDefiner.combineWithUserInfoApiUrl(subpath);
  }
}
