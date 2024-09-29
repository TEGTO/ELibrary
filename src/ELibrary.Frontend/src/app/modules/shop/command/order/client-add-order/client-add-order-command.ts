/* eslint-disable @typescript-eslint/no-explicit-any */
import { MatDialogRef } from "@angular/material/dialog";
import { Command, Order } from "../../../../shared";

export interface ClientAddOrderCommand extends Command {
    order: Order | undefined;
    matDialogRef: MatDialogRef<any, any> | undefined;
}