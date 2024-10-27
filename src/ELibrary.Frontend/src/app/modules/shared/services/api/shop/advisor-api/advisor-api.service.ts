import { Injectable } from '@angular/core';
import { catchError, Observable } from 'rxjs';
import { AdvisorQueryRequest, AdvisorResponse, BaseApiService } from '../../../..';

@Injectable({
  providedIn: 'root'
})
export class AdvisorApiService extends BaseApiService {

  sendQuery(req: AdvisorQueryRequest): Observable<AdvisorResponse> {
    return this.httpClient.post<AdvisorResponse>(this.combinePathWithAdvisorApiUrl(``), req).pipe(
      catchError((error) => this.handleError(error))
    );
  }

  private combinePathWithAdvisorApiUrl(subpath: string): string {
    return this.urlDefiner.combineWithShopApiUrl("/advisor" + subpath);
  }
}

