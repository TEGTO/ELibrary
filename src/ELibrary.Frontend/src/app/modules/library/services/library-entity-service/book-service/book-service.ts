import { Injectable } from "@angular/core";
import { Book, BookFilterRequest, CreateBookRequest, UpdateBookRequest } from "../../../../shared";
import { LibraryEntityService } from "../library-entity-service";

@Injectable({
    providedIn: 'root'
})
export abstract class BookService extends LibraryEntityService<
    Book,
    BookFilterRequest,
    CreateBookRequest,
    UpdateBookRequest> { }
