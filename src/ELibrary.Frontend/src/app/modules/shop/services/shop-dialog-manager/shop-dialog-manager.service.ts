/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { CartDialogComponent } from '../..';
import { ShopDialogManager } from './shop-dialog-manager';

@Injectable({
  providedIn: 'root'
})
export class ShopDialogManagerService implements ShopDialogManager {

  constructor(
    private readonly dialog: MatDialog
  ) {
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
}
