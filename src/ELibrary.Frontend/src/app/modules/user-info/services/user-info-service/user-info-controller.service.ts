import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GetCurrentUserResponse, UserInfoApiService } from '../../../shared';
import { UserInfoService } from './user-info-service';

@Injectable({
  providedIn: 'root'
})
export class UserInfoController implements UserInfoService {

  constructor(private readonly apiService: UserInfoApiService) { }

  getUserInfo(): Observable<GetCurrentUserResponse> {
    return this.apiService.getCurrentUser();
  }
}
