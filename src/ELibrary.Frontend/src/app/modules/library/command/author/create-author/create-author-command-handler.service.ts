import { Injectable } from '@angular/core';
import { take } from 'rxjs';
import { AuthorService, CreateAuthorCommand, LibraryDialogManager } from '../../..';
import { Author, CommandHandler, getDefaultAuthor, mapAuthorToCreateAuthorRequest } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class CreateAuthorCommandHandlerService extends CommandHandler<CreateAuthorCommand> {

  constructor(
    private readonly dialogManager: LibraryDialogManager,
    private readonly authorService: AuthorService,
  ) {
    super();
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  dispatch(command: CreateAuthorCommand): void {
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

}