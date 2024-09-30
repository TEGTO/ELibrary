/* eslint-disable @typescript-eslint/no-explicit-any */
import { MatDialogRef } from "@angular/material/dialog";

export abstract class DialogManager {
    abstract openConfirmMenu(): MatDialogRef<any>;
}
