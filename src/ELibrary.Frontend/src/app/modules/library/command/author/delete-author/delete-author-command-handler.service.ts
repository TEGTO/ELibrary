import { Injectable } from '@angular/core';
import { take } from 'rxjs';
import { AuthorService, DeleteAuthorCommand, LibraryDialogManager } from '../../..';
import { CommandHandler } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class DeleteAuthorCommandHandlerService extends CommandHandler<DeleteAuthorCommand> {

  constructor(
    private readonly dialogManager: LibraryDialogManager,
    private readonly authorService: AuthorService,
  ) {
    super();
  }

  dispatch(command: DeleteAuthorCommand): void {
    this.dialogManager.openConfirmMenu().afterClosed().pipe(
      take(1)
    ).subscribe((result: boolean) => {
      if (result === true) {
        this.authorService.deleteById(command.author.id);
      }
    });
  }
}
