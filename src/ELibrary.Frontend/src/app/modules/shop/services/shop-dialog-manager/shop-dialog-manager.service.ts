/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { BookStockReplenishmentComponent, CartDialogComponent, ClientChangeDialogComponent } from '../..';
import { Client, DialogManagerService } from '../../../shared';
import { ShopDialogManager } from './shop-dialog-manager';

@Injectable({
  providedIn: 'root'
})
export class ShopDialogManagerService implements ShopDialogManager {
  constructor(
    private readonly dialog: MatDialog,
    private readonly dialogManager: DialogManagerService
  ) {
  }

  openConfirmMenu(): MatDialogRef<any> {
    return this.dialogManager.openConfirmMenu();
  }
  openCartMenu(): MatDialogRef<any> {
    const dialogRef = this.dialog.open(CartDialogComponent, {
      height: '640px',
      width: '630px',
      maxHeight: '640px',
      maxWidth: '630px'
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
  openReplenishmentMenu(): MatDialogRef<any> {
    const dialogRef = this.dialog.open(BookStockReplenishmentComponent, {
      height: '430px',
      width: '550px',
      maxHeight: '430px',
      maxWidth: '550px',
    });
    return dialogRef;
  }
}
