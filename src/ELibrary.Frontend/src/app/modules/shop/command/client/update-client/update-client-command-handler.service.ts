import { Injectable } from '@angular/core';
import { ClientService, UpdateClientCommand } from '../../..';
import { CommandHandler, mapClientToUpdateClientRequest } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class UpdateClientCommandHandlerService extends CommandHandler<UpdateClientCommand> {
  constructor(
    private readonly clientService: ClientService,
  ) {
    super();
  }

  dispatch(command: UpdateClientCommand): void {
    const req = mapClientToUpdateClientRequest(command.client);
    this.clientService.updateClient(req);
  }

}