/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AuthorChangeDialogComponent, BookChangeDialogComponent, ConfirmMenuComponent, GenreChangeDialogComponent, PublisherChangeDialogComponent } from '../..';
import { AuthorResponse, BookResponse, GenreResponse, PublisherResponse } from '../../../shared';
import { LibraryDialogManager } from './library-dialog-manager';

@Injectable({
  providedIn: 'root'
})
export class LibraryDialogManagerService implements LibraryDialogManager {
  constructor(
    private readonly dialog: MatDialog
  ) { }

  openConfirmMenu(): MatDialogRef<any> {
    const dialogRef = this.dialog.open(ConfirmMenuComponent, {
      height: '200px',
      width: '450px',
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
      height: '230px',
      width: '450px',
      data: genre
    });
    return dialogRef;
  }
  openPublisherDetailsMenu(publisher: PublisherResponse): MatDialogRef<any> {
    const dialogRef = this.dialog.open(PublisherChangeDialogComponent, {
      height: '230px',
      width: '450px',
      data: publisher
    });
    return dialogRef;
  }
  openBookDetailsMenu(book: BookResponse): MatDialogRef<any> {
    const dialogRef = this.dialog.open(BookChangeDialogComponent, {
      height: '660px',
      width: '650px',
      data: book
    });
    return dialogRef;
  }
}