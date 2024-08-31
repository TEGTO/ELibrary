import { Injectable } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";
import { AuthorResponse } from "../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class LibraryDialogManager {
    abstract openConfirmMenu(): MatDialogRef<any>;
    abstract openDetailsMenu(author: AuthorResponse): MatDialogRef<any>;
}
