/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AuthorChangeDialogComponent, BookChangeDialogComponent, ConfirmMenuComponent, GenreChangeDialogComponent, PublisherChangeDialogComponent } from '../..';
import { Author, Book, Genre, Publisher } from '../../../shared';
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

  openAuthorDetailsMenu(author: Author): MatDialogRef<any> {
    const dialogRef = this.dialog.open(AuthorChangeDialogComponent, {
      height: '400px',
      width: '450px',
      data: author
    });
    return dialogRef;
  }
  openGenreDetailsMenu(genre: Genre): MatDialogRef<any> {
    const dialogRef = this.dialog.open(GenreChangeDialogComponent, {
      height: '230px',
      width: '450px',
      data: genre
    });
    return dialogRef;
  }
  openPublisherDetailsMenu(publisher: Publisher): MatDialogRef<any> {
    const dialogRef = this.dialog.open(PublisherChangeDialogComponent, {
      height: '230px',
      width: '450px',
      data: publisher
    });
    return dialogRef;
  }
  openBookDetailsMenu(book: Book | null): MatDialogRef<any> {
    const dialogRef = this.dialog.open(BookChangeDialogComponent, {
      height: '660px',
      width: '650px',
      data: book
    });
    return dialogRef;
  }
}