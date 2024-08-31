import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { AuthorResponse, CreateAuthorRequest, PaginatedRequest, UpdateAuthorRequest } from "../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class AuthorService {
    abstract getAuthorById(id: number): Observable<AuthorResponse>;
    abstract getAuthorsPaginated(request: PaginatedRequest): Observable<AuthorResponse[]>;
    abstract createAuthor(request: CreateAuthorRequest): void;
    abstract updateAuthor(request: UpdateAuthorRequest): void;
    abstract deleteAuthorById(id: number): void;
}
