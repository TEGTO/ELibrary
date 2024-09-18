/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unused-vars */
import { Injectable, OnDestroy } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Subject, switchMap, takeUntil } from 'rxjs';
import { AuthorService, BookService, GenreService, LibraryDialogManager, PublisherService } from '../..';
import { AuthorResponse, authorToCreateRequest, authorToUpdateRequest, BookResponse, bookToCreateRequest, bookToUpdateRequest, GenreResponse, genreToCreateRequest, genreToUpdateRequest, getDefaultAuthorResponse, getDefaultGenreResponse, getDefaultPublisherResponse, PublisherResponse, publisherToCreateRequest, publisherToUpdateRequest } from '../../../shared';
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
    const author: AuthorResponse = getDefaultAuthorResponse();

    this.dialogManager.openAuthorDetailsMenu(author).afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(author => {
      if (author) {
        const req = authorToCreateRequest(author);
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
        const req = authorToUpdateRequest(author);
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
    const genre: GenreResponse = getDefaultGenreResponse();

    this.dialogManager.openGenreDetailsMenu(genre).afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(genre => {
      if (genre) {
        const req = genreToCreateRequest(genre);
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
        const req = genreToUpdateRequest(genre);
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

  //#region  Publisher

  private createPublisher(dispatchedFrom: any, params: any[]) {
    const publisher: PublisherResponse = getDefaultPublisherResponse();

    this.dialogManager.openPublisherDetailsMenu(publisher).afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(publusher => {
      if (publusher) {
        const req = publisherToCreateRequest(publusher);
        this.publisherService.create(req);
      }
      this.cleanUp();
    });
  }
  private updatePublisher(dispatchedFrom: any, publisher: PublisherResponse, params: any[]) {
    this.dialogManager.openPublisherDetailsMenu(publisher).afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(publisher => {
      if (publisher) {
        const req = publisherToUpdateRequest(publisher);
        this.publisherService.update(req);
      }
      this.cleanUp();
    });
  }
  private deletePublisher(dispatchedFrom: any, publisher: PublisherResponse, params: any[]) {
    this.dialogManager.openConfirmMenu().afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(result => {
      if (result === true) {
        this.publisherService.deleteById(publisher.id);
      }
      this.cleanUp();
    });
  }

  //#endregion

  //#region  Book

  private createBook(dispatchedFrom: any, params: any[]) {
    this.dialogManager.openBookDetailsMenu(null).afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(book => {
      if (book) {
        console.log(book);
        const req = bookToCreateRequest(book);
        this.bookService.create(req);
      }
      this.cleanUp();
    });
  }
  private updateBook(dispatchedFrom: any, book: BookResponse, params: any[]) {

    this.bookService.getById(book.id).pipe(
      takeUntil(this.destroy$),
      switchMap((response) => {
        return this.dialogManager.openBookDetailsMenu(response).afterClosed();
      })
    ).subscribe(book => {
      if (book) {
        const req = bookToUpdateRequest(book);
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
    params.forEach(param => {
      if (param instanceof MatDialogRef) {
        param.close();
      }
    }
    );
  }
}
