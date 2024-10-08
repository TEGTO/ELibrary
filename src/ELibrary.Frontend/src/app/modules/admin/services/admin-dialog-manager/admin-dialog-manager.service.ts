/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AdminDialogManager, AdminRegisterUserDialogComponent } from '../..';
import { Client, DialogManagerService } from '../../../shared';
import { ClientChangeDialogComponent } from '../../../shop';

@Injectable({
  providedIn: 'root'
})
export class AdminDialogManagerService implements AdminDialogManager {

  constructor(
    private readonly dialog: MatDialog,
    private readonly dialogManager: DialogManagerService
  ) {
  }

  openConfirmMenu(): MatDialogRef<any> {
    return this.dialogManager.openConfirmMenu();
  }
  openRegisterMenu(): MatDialogRef<any> {
    const dialogRef = this.dialog.open(AdminRegisterUserDialogComponent, {
      height: '470px',
      width: '450px',
      maxHeight: '470px',
      maxWidth: '450px',
    });
    return dialogRef;
  }
  openClientChangeMenu(client: Client): MatDialogRef<any> {
    const dialogRef = this.dialog.open(ClientChangeDialogComponent, {
      height: '700px',
      width: '530px',
      maxHeight: '700px',
      maxWidth: '530px',
      data: client
    });
    return dialogRef;
  }
}