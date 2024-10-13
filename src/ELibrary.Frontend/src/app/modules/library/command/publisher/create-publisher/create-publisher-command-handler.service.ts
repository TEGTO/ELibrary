import { Injectable } from '@angular/core';
import { take } from 'rxjs';
import { CreatePublisherCommand, LibraryDialogManager, PublisherService } from '../../..';
import { CommandHandler, getDefaultPublisher, mapPublisherToCreatePublisherRequest, Publisher } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class CreatePublisherCommandHandlerService extends CommandHandler<CreatePublisherCommand> {

  constructor(
    private readonly dialogManager: LibraryDialogManager,
    private readonly publisherService: PublisherService,
  ) {
    super();
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  dispatch(command: CreatePublisherCommand): void {
    const publisher: Publisher = getDefaultPublisher();

    this.dialogManager.openPublisherDetailsMenu(publisher).afterClosed().pipe(
      take(1)
    ).subscribe(publusher => {
      if (publusher) {
        const req = mapPublisherToCreatePublisherRequest(publusher);
        this.publisherService.create(req);
      }
    });
  }

}