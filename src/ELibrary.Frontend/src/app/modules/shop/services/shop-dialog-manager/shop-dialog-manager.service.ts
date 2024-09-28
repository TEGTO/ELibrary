/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { BookStockReplenishmentComponent, CartDialogComponent, ClientChangeDialogComponent } from '../..';
import { Client, DialogManagerService } from '../../../shared';
import { ShopDialogManager } from './shop-dialog-manager';

@Injectable({
  providedIn: 'root'
})
export class ShopDialogManagerService extends DialogManagerService implements ShopDialogManager {
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
      height: '400px',
      width: '550px',
      maxHeight: '400px',
      maxWidth: '550px',
    });
    return dialogRef;
  }
}
