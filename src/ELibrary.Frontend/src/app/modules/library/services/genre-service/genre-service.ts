import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { CreateGenreRequest, GenreResponse, PaginatedRequest, UpdateGenreRequest } from "../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class GenreService {
    abstract getGenreById(id: number): Observable<GenreResponse>;
    abstract getGenresPaginated(request: PaginatedRequest): Observable<GenreResponse[]>;
    abstract getItemTotalAmount(): Observable<number>;
    abstract createGenre(request: CreateGenreRequest): void;
    abstract updateGenre(request: UpdateGenreRequest): void;
    abstract deleteGenreById(id: number): void;
}