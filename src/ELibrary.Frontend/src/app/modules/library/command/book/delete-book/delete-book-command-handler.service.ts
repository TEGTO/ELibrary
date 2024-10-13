import { Injectable } from '@angular/core';
import { take } from 'rxjs';
import { BookService, DeleteBookCommand, LibraryDialogManager } from '../../..';
import { CommandHandler } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class DeleteBookCommandHandlerService extends CommandHandler<DeleteBookCommand> {

  constructor(
    private readonly dialogManager: LibraryDialogManager,
    private readonly bookService: BookService,
  ) {
    super();
  }

  dispatch(command: DeleteBookCommand): void {
    this.dialogManager.openConfirmMenu().afterClosed().pipe(
      take(1)
    ).subscribe(result => {
      if (result === true) {
        this.bookService.deleteById(command.book.id);
      }
    });
  }
}