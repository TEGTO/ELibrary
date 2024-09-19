import { Injectable } from "@angular/core";
import { Author, CreateAuthorRequest, LibraryFilterRequest, UpdateAuthorRequest } from "../../../../shared";
import { LibraryEntityService } from "../library-entity-service";

@Injectable({
    providedIn: 'root'
})
export abstract class AuthorService extends LibraryEntityService<
    Author,
    LibraryFilterRequest,
    CreateAuthorRequest,
    UpdateAuthorRequest> { }
