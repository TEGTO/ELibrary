import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export abstract class LibraryEntityService<T, TFilterRequest, TCreateRequest, TUpdateRequest> {
    abstract getById(id: number): Observable<T>;
    abstract getPaginated(request: TFilterRequest): Observable<T[]>;
    abstract getItemTotalAmount(request: TFilterRequest): Observable<number>;
    abstract create(request: TCreateRequest): void;
    abstract update(request: TUpdateRequest): void;
    abstract deleteById(id: number): void;
}
