import { Injectable } from "@angular/core";
import { CreateGenreRequest, Genre, LibraryFilterRequest, UpdateGenreRequest } from "../../../../shared";
import { LibraryEntityService } from "../library-entity-service";

@Injectable({
    providedIn: 'root'
})
export abstract class GenreService extends LibraryEntityService<
    Genre,
    LibraryFilterRequest,
    CreateGenreRequest,
    UpdateGenreRequest> { }