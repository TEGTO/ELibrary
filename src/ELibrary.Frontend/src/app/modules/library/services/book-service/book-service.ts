import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BookResponse, CreateBookRequest, PaginatedRequest, UpdateBookRequest } from "../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class BookService {
    abstract getBookById(id: number): Observable<BookResponse>;
    abstract getBooksPaginated(request: PaginatedRequest): Observable<BookResponse[]>;
    abstract createBook(request: CreateBookRequest): void;
    abstract updateBook(request: UpdateBookRequest): void;
    abstract deleteBookById(id: number): void;
}
