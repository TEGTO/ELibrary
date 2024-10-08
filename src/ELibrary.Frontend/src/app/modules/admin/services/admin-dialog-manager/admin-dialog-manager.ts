/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";
import { Client, DialogManager } from "../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class AdminDialogManager implements DialogManager {
    abstract openConfirmMenu(): MatDialogRef<any>;
    abstract openRegisterMenu(): MatDialogRef<any>;
    abstract openClientChangeMenu(client: Client): MatDialogRef<any>;
}