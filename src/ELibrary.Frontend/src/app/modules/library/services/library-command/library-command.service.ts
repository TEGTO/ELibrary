/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unused-vars */
import { Injectable, OnDestroy } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Subject, switchMap, take, takeUntil } from 'rxjs';
import { AuthorService, BookService, GenreService, LibraryDialogManager, PublisherService } from '../..';
import { Author, Book, Genre, getDefaultAuthor, getDefaultGenre, getDefaultPublisher, mapAuthorToCreateAuthorRequest, mapAuthorToUpdateAuthorRequest, mapBookToCreateBookRequest, mapBookToUpdateBookRequest, mapGenreToCreateGenreRequest, mapGenreToUpdateGenreRequest, mapPublisherToCreatePublisherRequest, mapPublisherToUpdatePublisherRequest, Publisher } from '../../../shared';
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
    private readonly publisherService: PublisherService,
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
            this.createPublisher(dispatchedFrom, params);
            break;
          case LibraryCommandType.Update:
            this.updatePublisher(dispatchedFrom, params[0], params);
            break;
          case LibraryCommandType.Delete:
            this.deletePublisher(dispatchedFrom, params[0], params);
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
    const author: Author = getDefaultAuthor();

    this.dialogManager.openAuthorDetailsMenu(author).afterClosed().pipe(
      take(1)
    ).subscribe(author => {
      if (author) {
        const req = mapAuthorToCreateAuthorRequest(author);
        this.authorService.create(req);
      }
    });
  }
  private updateAuthor(dispatchedFrom: any, author: Author, params: any[]) {
    this.dialogManager.openAuthorDetailsMenu(author).afterClosed().pipe(
      take(1)
    ).subscribe(author => {
      if (author) {
        const req = mapAuthorToUpdateAuthorRequest(author);
        this.authorService.update(req);
      }
    });
  }
  private deleteAuthor(dispatchedFrom: any, author: Author, params: any[]) {
    this.dialogManager.openConfirmMenu().afterClosed().pipe(
      take(1)
    ).subscribe(result => {
      if (result === true) {
        this.authorService.deleteById(author.id);
      }
    });
  }

  //#endregion

  //#region Genre

  private createGenre(dispatchedFrom: any, params: any[]) {
    const genre: Genre = getDefaultGenre();

    this.dialogManager.openGenreDetailsMenu(genre).afterClosed().pipe(
      take(1)
    ).subscribe(genre => {
      if (genre) {
        const req = mapGenreToCreateGenreRequest(genre);
        this.genreService.create(req);
      }
    });
  }
  private updateGenre(dispatchedFrom: any, genre: Genre, params: any[]) {
    this.dialogManager.openGenreDetailsMenu(genre).afterClosed().pipe(
      take(1)
    ).subscribe(genre => {
      if (genre) {
        const req = mapGenreToUpdateGenreRequest(genre);
        this.genreService.update(req);
      }
    });
  }
  private deleteGenre(dispatchedFrom: any, genre: Genre, params: any[]) {
    this.dialogManager.openConfirmMenu().afterClosed().pipe(
      take(1)
    ).subscribe(result => {
      if (result === true) {
        this.genreService.deleteById(genre.id);
      }
    });
  }

  //#endregion

  //#region  Publisher

  private createPublisher(dispatchedFrom: any, params: any[]) {
    const publisher: Publisher = getDefaultPublisher();

    this.dialogManager.openPublisherDetailsMenu(publisher).afterClosed().pipe(
      take(1)
    ).subscribe(publusher => {
      if (publusher) {
        const req = mapPublisherToCreatePublisherRequest(publusher);
        this.publisherService.create(req);
      }
    });
  }
  private updatePublisher(dispatchedFrom: any, publisher: Publisher, params: any[]) {
    this.dialogManager.openPublisherDetailsMenu(publisher).afterClosed().pipe(
      take(1)
    ).subscribe(publisher => {
      if (publisher) {
        const req = mapPublisherToUpdatePublisherRequest(publisher);
        this.publisherService.update(req);
      }
    });
  }
  private deletePublisher(dispatchedFrom: any, publisher: Publisher, params: any[]) {
    this.dialogManager.openConfirmMenu().afterClosed().pipe(
      take(1)
    ).subscribe(result => {
      if (result === true) {
        this.publisherService.deleteById(publisher.id);
      }
    });
  }

  //#endregion

  //#region  Book

  private createBook(dispatchedFrom: any, params: any[]) {
    this.dialogManager.openBookDetailsMenu(null).afterClosed().pipe(
      take(1)
    ).subscribe(book => {
      if (book) {
        console.log(book);
        const req = mapBookToCreateBookRequest(book);
        this.bookService.create(req);
      }
    });
  }
  private updateBook(dispatchedFrom: any, book: Book, params: any[]) {

    this.bookService.getById(book.id).pipe(
      takeUntil(this.destroy$),
      switchMap((response) => {
        return this.dialogManager.openBookDetailsMenu(response).afterClosed();
      })
    ).subscribe(book => {
      if (book) {
        const req = mapBookToUpdateBookRequest(book);
        this.bookService.update(req);
      }
      this.cleanUp();
    });
  }
  private deleteBook(dispatchedFrom: any, book: Book, params: any[]) {
    this.dialogManager.openConfirmMenu().afterClosed().pipe(
      take(1)
    ).subscribe(result => {
      if (result === true) {
        this.bookService.deleteById(book.id);
      }
    });
  }

  //#endregion

  private closeDialogIfPresent(params: any[]): void {
    params.forEach(param => {
      if (param instanceof MatDialogRef) {
        param.close();
      }
    }
    );
  }
}
