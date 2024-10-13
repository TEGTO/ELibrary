import { Injectable } from '@angular/core';
import { take } from 'rxjs';
import { DeletePublisherCommand, LibraryDialogManager, PublisherService } from '../../..';
import { CommandHandler } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class DeletePublisherCommandHandlerService extends CommandHandler<DeletePublisherCommand> {

  constructor(
    private readonly dialogManager: LibraryDialogManager,
    private readonly publisherService: PublisherService,
  ) {
    super();
  }

  dispatch(command: DeletePublisherCommand): void {
    this.dialogManager.openConfirmMenu().afterClosed().pipe(
      take(1)
    ).subscribe(result => {
      if (result === true) {
        this.publisherService.deleteById(command.publisher.id);
      }
    });
  }
}
