import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { AdminGetUserFilter, AdminUser, AdminUserResponse, AdminUserUpdateDataRequest, BaseApiService, mapAdminUserResponseToAdminUser, UserUpdateRequest } from '../../..';

@Injectable({
  providedIn: 'root'
})
export class UserApiService extends BaseApiService {

  //#region User

  updateUser(req: UserUpdateRequest): Observable<HttpResponse<void>> {
    return this.httpClient.put<void>(this.combinePathWithUserApiUrl(`/update`), req, { observe: 'response' }).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  deleteUser(): Observable<HttpResponse<void>> {
    return this.httpClient.delete<void>(this.combinePathWithUserApiUrl(`/delete`), { observe: 'response' }).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }

  //#endregion

  //#region Admin

  adminGetUser(info: string): Observable<AdminUser> {
    return this.httpClient.get<AdminUserResponse>(this.combinePathWithUserApiUrl(`/admin/users/${info}`)).pipe(
      map((response) => mapAdminUserResponseToAdminUser(response)),
      catchError((resp) => this.handleError(resp))
    );
  }
  adminGetPaginatedUsers(req: AdminGetUserFilter): Observable<AdminUser[]> {
    return this.httpClient.post<AdminUserResponse[]>(this.combinePathWithUserApiUrl(`/admin/users`), req).pipe(
      map((response) => response.map(x => mapAdminUserResponseToAdminUser(x))),
      catchError((resp) => this.handleError(resp))
    );
  }
  adminGetPaginatedUserAmount(req: AdminGetUserFilter): Observable<number> {
    return this.httpClient.post<number>(this.combinePathWithUserApiUrl(`/admin/users/amount`), req).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }
  adminUpdateUser(req: AdminUserUpdateDataRequest): Observable<AdminUser> {
    return this.httpClient.put<AdminUserResponse>(this.combinePathWithUserApiUrl(`/admin/update`), req).pipe(
      map((response) => mapAdminUserResponseToAdminUser(response)),
      catchError((resp) => this.handleError(resp))
    );
  }
  adminDeleteUser(id: string): Observable<HttpResponse<void>> {
    return this.httpClient.delete<void>(this.combinePathWithUserApiUrl(`/admin/delete/${id}`), { observe: 'response' }).pipe(
      catchError((resp) => this.handleError(resp))
    );
  }

  //#endregion

  private combinePathWithUserApiUrl(subpath: string): string {
    return this.urlDefiner.combineWithUserApiUrl("/user" + subpath);
  }
}
