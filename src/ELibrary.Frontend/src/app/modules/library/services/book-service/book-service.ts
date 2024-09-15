import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BookFilterRequest, BookResponse, CreateBookRequest, UpdateBookRequest } from "../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class BookService {
    abstract getById(id: number): Observable<BookResponse>;
    abstract getPaginated(request: BookFilterRequest): Observable<BookResponse[]>;
    abstract getItemTotalAmount(request: BookFilterRequest): Observable<number>;
    abstract create(request: CreateBookRequest): void;
    abstract update(request: UpdateBookRequest): void;
    abstract deleteById(id: number): void;
}
