/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";
import { AuthorResponse, BookResponse, GenreResponse, PublisherResponse } from "../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class LibraryDialogManager {
    abstract openConfirmMenu(): MatDialogRef<any>;
    abstract openBookDetailsMenu(book: BookResponse | null): MatDialogRef<any>;
    abstract openAuthorDetailsMenu(author: AuthorResponse): MatDialogRef<any>;
    abstract openGenreDetailsMenu(genre: GenreResponse): MatDialogRef<any>;
    abstract openPublisherDetailsMenu(publisher: PublisherResponse): MatDialogRef<any>;
}
