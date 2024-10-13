/* eslint-disable @typescript-eslint/no-explicit-any */
import { MatDialogRef } from "@angular/material/dialog";
import { Command } from "../../../../shared";

export interface ClientStartAddingOrderCommand extends Command {
    matDialogRef: MatDialogRef<any> | undefined
}