import { Injectable } from '@angular/core';
import { ClientService, DeleteClientCommand } from '../../..';
import { CommandHandler } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class DeleteClientCommandHandlerService extends CommandHandler<DeleteClientCommand> {
  constructor(
    private readonly clientService: ClientService,
  ) {
    super();
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  dispatch(command: DeleteClientCommand): void {
    this.clientService.deleteClient();
  }

}