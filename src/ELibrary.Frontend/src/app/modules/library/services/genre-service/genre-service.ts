import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { CreateGenreRequest, GenreResponse, LibraryFilterRequest, UpdateGenreRequest } from "../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class GenreService {
    abstract getById(id: number): Observable<GenreResponse>;
    abstract getPaginated(request: LibraryFilterRequest): Observable<GenreResponse[]>;
    abstract getItemTotalAmount(request: LibraryFilterRequest): Observable<number>;
    abstract create(request: CreateGenreRequest): void;
    abstract update(request: UpdateGenreRequest): void;
    abstract deleteById(id: number): void;
}