import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AuthorChangeDialogComponent, BookChangeDialogComponent, ConfirmMenuComponent, GenreChangeDialogComponent } from '../..';
import { AuthorResponse, BookResponse, GenreResponse } from '../../../shared';
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

  openBookDetailsMenu(book: BookResponse): MatDialogRef<any> {
    const dialogRef = this.dialog.open(BookChangeDialogComponent, {
      height: '460px',
      width: '450px',
      data: book
    });
    return dialogRef;
  }
  openAuthorDetailsMenu(author: AuthorResponse): MatDialogRef<any> {
    const dialogRef = this.dialog.open(AuthorChangeDialogComponent, {
      height: '400px',
      width: '450px',
      data: author
    });
    return dialogRef;
  }
  openGenreDetailsMenu(genre: GenreResponse): MatDialogRef<any> {
    const dialogRef = this.dialog.open(GenreChangeDialogComponent, {
      height: '220px',
      width: '450px',
      data: genre
    });
    return dialogRef;
  }
}