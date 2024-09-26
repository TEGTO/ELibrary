/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";
import { DialogManager } from "../../../shared/services/dialog-manager/dialog-manager";

@Injectable({
    providedIn: 'root'
})
export abstract class AuthenticationDialogManager extends DialogManager {
    abstract openLoginMenu(): MatDialogRef<any>;
    abstract openRegisterMenu(): MatDialogRef<any>;
}
