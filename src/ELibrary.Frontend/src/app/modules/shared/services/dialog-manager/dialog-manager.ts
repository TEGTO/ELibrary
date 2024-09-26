/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";

@Injectable({
    providedIn: 'root'
})
export abstract class DialogManager {
    abstract openConfirmMenu(): MatDialogRef<any>;
}
