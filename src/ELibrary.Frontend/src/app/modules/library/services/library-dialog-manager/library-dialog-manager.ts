/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";
import { Author, Book, Genre, Publisher } from "../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class LibraryDialogManager {
    abstract openConfirmMenu(): MatDialogRef<any>;
    abstract openBookDetailsMenu(book: Book | null): MatDialogRef<any>;
    abstract openAuthorDetailsMenu(author: Author): MatDialogRef<any>;
    abstract openGenreDetailsMenu(genre: Genre): MatDialogRef<any>;
    abstract openPublisherDetailsMenu(publisher: Publisher): MatDialogRef<any>;
}
