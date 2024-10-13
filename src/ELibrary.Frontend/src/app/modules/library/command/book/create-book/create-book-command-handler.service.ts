import { Injectable } from '@angular/core';
import { take } from 'rxjs';
import { BookService, CreateBookCommand, LibraryDialogManager } from '../../..';
import { CommandHandler, mapBookToCreateBookRequest } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class CreateBookCommandHandlerService extends CommandHandler<CreateBookCommand> {

  constructor(
    private readonly dialogManager: LibraryDialogManager,
    private readonly bookService: BookService,
  ) {
    super();
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  dispatch(command: CreateBookCommand): void {
    this.dialogManager.openBookDetailsMenu(null).afterClosed().pipe(
      take(1)
    ).subscribe(book => {
      if (book) {
        const req = mapBookToCreateBookRequest(book);
        this.bookService.create(req);
      }
    });
  }

}