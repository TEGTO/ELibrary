import { Injectable } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";
import { AuthorResponse, BookResponse, GenreResponse } from "../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class LibraryDialogManager {
    abstract openConfirmMenu(): MatDialogRef<any>;
    abstract openBookDetailsMenu(book: BookResponse): MatDialogRef<any>;
    abstract openAuthorDetailsMenu(author: AuthorResponse): MatDialogRef<any>;
    abstract openGenreDetailsMenu(genre: GenreResponse): MatDialogRef<any>;
}
