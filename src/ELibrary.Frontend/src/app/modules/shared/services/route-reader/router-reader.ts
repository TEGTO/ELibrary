import { Injectable } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export abstract class RouteReader {
    abstract readId<T>(route: ActivatedRoute, fetchFn: (id: string) => Observable<T>): Observable<T>;
    abstract readIdInt<T>(route: ActivatedRoute, fetchFn: (id: number) => Observable<T>): Observable<T>;
}
