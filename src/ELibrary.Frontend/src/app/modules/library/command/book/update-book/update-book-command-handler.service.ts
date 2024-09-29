import { Injectable, OnDestroy } from '@angular/core';
import { Subject, switchMap, takeUntil } from 'rxjs';
import { BookService, LibraryDialogManager, UpdateBookCommand } from '../../..';
import { CommandHandler, mapBookToUpdateBookRequest } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class UpdateBookCommandHandlerService extends CommandHandler<UpdateBookCommand> implements OnDestroy {
  private destroy$ = new Subject<void>();

  constructor(
    private readonly dialogManager: LibraryDialogManager,
    private readonly bookService: BookService,
  ) {
    super();
  }

  ngOnDestroy(): void {
    this.cleanUp();
  }
  cleanUp() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  dispatch(command: UpdateBookCommand): void {
    this.bookService.getById(command.book.id).pipe(
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

}