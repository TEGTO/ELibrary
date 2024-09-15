import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { AuthorResponse, CreateAuthorRequest, LibraryFilterRequest, UpdateAuthorRequest } from "../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class AuthorService {
    abstract getById(id: number): Observable<AuthorResponse>;
    abstract getPaginated(request: LibraryFilterRequest): Observable<AuthorResponse[]>;
    abstract getItemTotalAmount(request: LibraryFilterRequest): Observable<number>;
    abstract create(request: CreateAuthorRequest): void;
    abstract update(request: UpdateAuthorRequest): void;
    abstract deleteById(id: number): void;
}
