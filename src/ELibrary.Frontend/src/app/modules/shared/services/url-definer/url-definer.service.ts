import { Injectable } from '@angular/core';
import { environment } from '../../../../../environment/environment';

@Injectable({
  providedIn: 'root'
})
export class URLDefiner {
  combineWithUserApiUrl(subpath: string): string {
    return environment.userApi + subpath;
  }
  combineWithLibraryApiUrl(subpath: string): string {
    return environment.libraryApi + subpath;
  }
  combineWithShopApiUrl(subpath: string): string {
    return environment.shopApi + subpath;
  }
}
