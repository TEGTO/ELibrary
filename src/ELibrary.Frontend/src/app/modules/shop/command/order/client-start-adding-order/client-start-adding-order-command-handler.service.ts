import { Injectable } from '@angular/core';
import { ClientStartAddingOrderCommand } from '../../..';
import { CommandHandler, getClientMakeOrderPath, RedirectorService } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class ClientStartAddingOrderCommandHandlerService extends CommandHandler<ClientStartAddingOrderCommand> {

  constructor(
    private readonly redirector: RedirectorService,
  ) {
    super();
  }

  dispatch(command: ClientStartAddingOrderCommand): void {
    if (command.matDialogRef) {
      command.matDialogRef.close();
    }
    this.redirector.redirectTo(getClientMakeOrderPath());
  }
}
