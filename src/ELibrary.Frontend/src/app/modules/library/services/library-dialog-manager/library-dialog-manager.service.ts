import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AuthorChangeDialogComponent, ConfirmMenuComponent } from '../..';
import { AuthorResponse } from '../../../shared';
import { LibraryDialogManager } from './library-dialog-manager';

@Injectable({
  providedIn: 'root'
})
export class LibraryDialogManagerService implements LibraryDialogManager {
  constructor(
    private readonly dialog: MatDialog
  ) { }

  openConfirmMenu(): MatDialogRef<any> {
    var dialogRef: MatDialogRef<any, any>;
    dialogRef = this.dialog.open(ConfirmMenuComponent, {
      height: '200px',
      width: '450px',
    });
    return dialogRef;
  }

  openDetailsMenu(author: AuthorResponse): MatDialogRef<any> {
    const dialogRef = this.dialog.open(AuthorChangeDialogComponent, {
      height: '400px',
      width: '450px',
      data: author
    });
    return dialogRef;
  }
}