import { Injectable } from '@angular/core';
import { take } from 'rxjs';
import { GenreService, LibraryDialogManager, UpdateGenreCommand } from '../../..';
import { CommandHandler, mapGenreToUpdateGenreRequest } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class UpdateGenreCommandHandlerService extends CommandHandler<UpdateGenreCommand> {

  constructor(
    private readonly dialogManager: LibraryDialogManager,
    private readonly genreService: GenreService,
  ) {
    super();
  }

  dispatch(command: UpdateGenreCommand): void {
    this.dialogManager.openGenreDetailsMenu(command.genre).afterClosed().pipe(
      take(1)
    ).subscribe(genre => {
      if (genre) {
        const req = mapGenreToUpdateGenreRequest(genre);
        this.genreService.update(req);
      }
    });
  }

}
