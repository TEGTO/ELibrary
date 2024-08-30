import { Injectable } from '@angular/core';
import { environment } from '../../../../../environment/environment';

@Injectable({
  providedIn: 'root'
})
export class URLDefiner {
  combineWithAuthApiUrl(subpath: string): string {
    return environment.userApi + "/auth" + subpath;
  }
  combineWithUserInfoApiUrl(subpath: string): string {
    return environment.userApi + "/userinfo" + subpath;
  }
}
