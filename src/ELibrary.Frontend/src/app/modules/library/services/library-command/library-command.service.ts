import { Injectable, OnDestroy } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Subject, takeUntil } from 'rxjs';
import { AuthorService, BookService, GenreService, LibraryDialogManager } from '../..';
import { AuthorResponse, authorToCreateRequest, authorToUpdateRequest, BookResponse, bookToCreateRequest, bookToUpdateRequest, GenreResponse, genreToCreateRequest, genreToUpdateRequest } from '../../../shared';
import { LibraryCommand, LibraryCommandObject, LibraryCommandType } from './library-command';

@Injectable({
  providedIn: 'root'
})
export class LibraryCommandService implements LibraryCommand, OnDestroy {
  private destroy$ = new Subject<void>();

  constructor(
    private readonly dialogManager: LibraryDialogManager,
    private readonly authorService: AuthorService,
    private readonly genreService: GenreService,
    private readonly bookService: BookService,
  ) { }

  ngOnDestroy(): void {
    this.cleanUp();
  }

  cleanUp(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  dispatchCommand(commandObject: LibraryCommandObject, commandType: LibraryCommandType, dispatchedFrom: any, ...params: any): void {
    switch (commandObject) {
      case LibraryCommandObject.Author:
        switch (commandType) {
          case LibraryCommandType.Create:
            this.createAuthor(dispatchedFrom, params);
            break;
          case LibraryCommandType.Update:
            this.updateAuthor(dispatchedFrom, params[0], params);
            break;
          case LibraryCommandType.Delete:
            this.deleteAuthor(dispatchedFrom, params[0], params);
            break;
          default:
            break;
        }
        break;
      case LibraryCommandObject.Genre:
        switch (commandType) {
          case LibraryCommandType.Create:
            this.createGenre(dispatchedFrom, params);
            break;
          case LibraryCommandType.Update:
            this.updateGenre(dispatchedFrom, params[0], params);
            break;
          case LibraryCommandType.Delete:
            this.deleteGenre(dispatchedFrom, params[0], params);
            break;
          default:
            break;
        }
        break;
      case LibraryCommandObject.Publisher:
        switch (commandType) {
          case LibraryCommandType.Create:
            break;
          case LibraryCommandType.Update:
            break;
          case LibraryCommandType.Delete:
            break;
          default:
            break;
        }
        break;

      case LibraryCommandObject.Book:
        switch (commandType) {
          case LibraryCommandType.Create:
            this.createBook(dispatchedFrom, params);
            break;
          case LibraryCommandType.Update:
            this.updateBook(dispatchedFrom, params[0], params);
            break;
          case LibraryCommandType.Delete:
            this.deleteBook(dispatchedFrom, params[0], params);
            break;
          default:
            break;
        }
        break;
      default:
        break;
    }
  }

  //#region  Author

  private createAuthor(dispatchedFrom: any, params: any[]) {
    let author: AuthorResponse = {
      id: 0,
      name: "",
      lastName: "",
      dateOfBirth: new Date()
    }

    this.dialogManager.openAuthorDetailsMenu(author).afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(author => {
      if (author) {
        let req = authorToCreateRequest(author);
        this.authorService.create(req);
      }
      this.cleanUp();
    });
  }
  private updateAuthor(dispatchedFrom: any, author: AuthorResponse, params: any[]) {
    this.dialogManager.openAuthorDetailsMenu(author).afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(author => {
      if (author) {
        let req = authorToUpdateRequest(author);
        this.authorService.update(req);
      }
      this.cleanUp();
    });
  }
  private deleteAuthor(dispatchedFrom: any, author: AuthorResponse, params: any[]) {
    this.dialogManager.openConfirmMenu().afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(result => {
      if (result === true) {
        this.authorService.deleteById(author.id);
      }
      this.cleanUp();
    });
  }

  //#endregion

  //#region Genre

  private createGenre(dispatchedFrom: any, params: any[]) {
    let genre: GenreResponse = {
      id: 0,
      name: "",
    }

    this.dialogManager.openGenreDetailsMenu(genre).afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(genre => {
      if (genre) {
        let req = genreToCreateRequest(genre);
        this.genreService.create(req);
      }
      this.cleanUp();
    });
  }
  private updateGenre(dispatchedFrom: any, genre: GenreResponse, params: any[]) {
    this.dialogManager.openGenreDetailsMenu(genre).afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(genre => {
      if (genre) {
        let req = genreToUpdateRequest(genre);
        this.genreService.update(req);
      }
      this.cleanUp();
    });
  }
  private deleteGenre(dispatchedFrom: any, genre: GenreResponse, params: any[]) {
    this.dialogManager.openConfirmMenu().afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(result => {
      if (result === true) {
        this.genreService.deleteById(genre.id);
      }
      this.cleanUp();
    });
  }

  //#endregion

  //#region  Book

  private createBook(dispatchedFrom: any, params: any[]) {
    let book: BookResponse = {
      id: 0,
      name: "",
      publicationDate: new Date(),
      price: 0,
      coverType: 0,
      pageAmount: 0,
      stockAmount: 0,
      author: {
        id: 0,
        name: "",
        lastName: "",
        dateOfBirth: new Date(),
      },
      genre: {
        id: 0,
        name: ""
      },
      publisher: {
        id: 0,
        name: ""
      }
    }

    this.dialogManager.openBookDetailsMenu(book).afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(book => {
      if (book) {
        let req = bookToCreateRequest(book);
        this.bookService.create(req);
      }
      this.cleanUp();
    });
  }
  private updateBook(dispatchedFrom: any, book: BookResponse, params: any[]) {
    this.dialogManager.openBookDetailsMenu(book).afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(book => {
      if (book) {
        let req = bookToUpdateRequest(book);
        this.bookService.update(req);
      }
      this.cleanUp();
    });
  }
  private deleteBook(dispatchedFrom: any, book: BookResponse, params: any[]) {
    this.dialogManager.openConfirmMenu().afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(result => {
      if (result === true) {
        this.bookService.deleteById(book.id);
      }
      this.cleanUp();
    });
  }

  //#endregion

  private closeDialogIfPresent(params: any[]): void {
    for (var i = 0; i < params.length; i++) {
      if (params[i] instanceof MatDialogRef) {
        params[i].close();
      }
    }
  }
}
