import { Injectable } from "@angular/core";
import { CreateGenreRequest, GenreResponse, LibraryFilterRequest, UpdateGenreRequest } from "../../../../shared";
import { LibraryEntityService } from "../library-entity-service";

@Injectable({
    providedIn: 'root'
})
export abstract class GenreService extends LibraryEntityService<
    GenreResponse,
    LibraryFilterRequest,
    CreateGenreRequest,
    UpdateGenreRequest> { }