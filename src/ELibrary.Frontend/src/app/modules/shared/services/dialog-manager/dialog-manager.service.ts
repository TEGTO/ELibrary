/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { ConfirmMenuComponent } from '../..';
import { DialogManager } from './dialog-manager';

@Injectable({
  providedIn: 'root'
})
export abstract class DialogManagerService implements DialogManager {
  constructor(
    private readonly dialog: MatDialog
  ) {
  }

  openConfirmMenu(): MatDialogRef<any> {
    const dialogRef = this.dialog.open(ConfirmMenuComponent, {
      maxHeight: '200px',
      width: '450px',
    });
    return dialogRef;
  }

}
