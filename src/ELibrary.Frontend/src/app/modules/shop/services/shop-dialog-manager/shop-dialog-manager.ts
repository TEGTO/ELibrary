/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";
import { Client, DialogManager } from "../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class ShopDialogManager implements DialogManager {
    abstract openCartMenu(): MatDialogRef<any>;
    abstract openClientChangeMenu(client: Client): MatDialogRef<any>;
    abstract openReplenishmentMenu(): MatDialogRef<any>;
    abstract openConfirmMenu(): MatDialogRef<any>;
}
