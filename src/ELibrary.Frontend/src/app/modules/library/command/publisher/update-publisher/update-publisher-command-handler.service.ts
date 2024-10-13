import { Injectable } from '@angular/core';
import { take } from 'rxjs';
import { LibraryDialogManager, PublisherService, UpdatePublisherCommand } from '../../..';
import { CommandHandler, mapPublisherToUpdatePublisherRequest } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class UpdatePublisherCommandHandlerService extends CommandHandler<UpdatePublisherCommand> {

  constructor(
    private readonly dialogManager: LibraryDialogManager,
    private readonly publisherService: PublisherService,
  ) {
    super();
  }

  dispatch(command: UpdatePublisherCommand): void {
    this.dialogManager.openPublisherDetailsMenu(command.publisher).afterClosed().pipe(
      take(1)
    ).subscribe(publisher => {
      if (publisher) {
        const req = mapPublisherToUpdatePublisherRequest(publisher);
        this.publisherService.update(req);
      }
    });
  }

}