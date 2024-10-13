import { Injectable } from '@angular/core';
import { take } from 'rxjs';
import { DeleteGenreCommand, GenreService, LibraryDialogManager } from '../../..';
import { CommandHandler } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class DeleteGenreCommandHandlerService extends CommandHandler<DeleteGenreCommand> {

  constructor(
    private readonly dialogManager: LibraryDialogManager,
    private readonly genreService: GenreService,
  ) {
    super();
  }

  dispatch(command: DeleteGenreCommand): void {
    this.dialogManager.openConfirmMenu().afterClosed().pipe(
      take(1)
    ).subscribe(result => {
      if (result === true) {
        this.genreService.deleteById(command.genre.id);
      }
    });
  }
}
