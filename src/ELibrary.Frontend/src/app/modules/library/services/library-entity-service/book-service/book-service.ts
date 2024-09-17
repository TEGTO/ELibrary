import { Injectable } from "@angular/core";
import { BookFilterRequest, BookResponse, CreateBookRequest, UpdateBookRequest } from "../../../../shared";
import { LibraryEntityService } from "../library-entity-service";

@Injectable({
    providedIn: 'root'
})
export abstract class BookService extends LibraryEntityService<
    BookResponse,
    BookFilterRequest,
    CreateBookRequest,
    UpdateBookRequest> { }
