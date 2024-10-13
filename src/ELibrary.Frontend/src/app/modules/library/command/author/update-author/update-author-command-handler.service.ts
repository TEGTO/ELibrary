import { Injectable } from '@angular/core';
import { take } from 'rxjs';
import { AuthorService, LibraryDialogManager, UpdateAuthorCommand } from '../../..';
import { CommandHandler, mapAuthorToUpdateAuthorRequest } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class UpdateAuthorCommandHandlerService extends CommandHandler<UpdateAuthorCommand> {

  constructor(
    private readonly dialogManager: LibraryDialogManager,
    private readonly authorService: AuthorService,
  ) {
    super();
  }

  dispatch(command: UpdateAuthorCommand): void {
    this.dialogManager.openAuthorDetailsMenu(command.author).afterClosed().pipe(
      take(1)
    ).subscribe(author => {
      if (author) {
        const req = mapAuthorToUpdateAuthorRequest(author);
        this.authorService.update(req);
      }
    });
  }

}