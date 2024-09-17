import { Injectable } from "@angular/core";
import { AuthorResponse, CreateAuthorRequest, LibraryFilterRequest, UpdateAuthorRequest } from "../../../../shared";
import { LibraryEntityService } from "../library-entity-service";

@Injectable({
    providedIn: 'root'
})
export abstract class AuthorService extends LibraryEntityService<
    AuthorResponse,
    LibraryFilterRequest,
    CreateAuthorRequest,
    UpdateAuthorRequest> { }
