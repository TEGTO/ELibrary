import { Injectable } from '@angular/core';
import { take } from 'rxjs';
import { CreateGenreCommand, GenreService, LibraryDialogManager } from '../../..';
import { CommandHandler, Genre, getDefaultGenre, mapGenreToCreateGenreRequest } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class CreateGenreCommandHandlerService extends CommandHandler<CreateGenreCommand> {

  constructor(
    private readonly dialogManager: LibraryDialogManager,
    private readonly genreService: GenreService,
  ) {
    super();
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  dispatch(command: CreateGenreCommand): void {
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

}