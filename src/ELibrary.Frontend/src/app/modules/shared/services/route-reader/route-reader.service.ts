import { Injectable } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { catchError, map, Observable, of, switchMap } from 'rxjs';
import { RedirectorService } from '../..';
import { RouteReader } from './router-reader';

@Injectable({
  providedIn: 'root'
})
export class RouteReaderService extends RouteReader {

  constructor(
    private readonly redirectService: RedirectorService,
  ) {
    super();
  }

  readId<T>(route: ActivatedRoute, fetchFn: (id: string) => Observable<T>): Observable<T> {
    return route.paramMap.pipe(
      map(params => params.get('id')),
      switchMap(id => {
        if (!id) {
          this.redirectService.redirectToHome();
          return of();
        }

        return fetchFn(id).pipe(
          catchError(() => {
            this.redirectService.redirectToHome();
            return of();
          })
        )
      }))
  }
  readIdInt<T>(route: ActivatedRoute, fetchFn: (id: number) => Observable<T>): Observable<T> {
    return route.paramMap.pipe(
      map(params => params.get('id')),
      switchMap(id => {
        if (!id) {
          this.redirectService.redirectToHome();
          return of();
        }

        const intId = parseInt(id, 10);
        if (isNaN(intId)) {
          this.redirectService.redirectToHome();
          return of();
        }
        return fetchFn(intId).pipe(
          catchError(() => {
            this.redirectService.redirectToHome();
            return of();
          })
        )
      }))
  }
}
