import { Injectable } from '@angular/core';
import { take } from 'rxjs';
import { AddClientCommand, ClientService, ShopDialogManager } from '../../..';
import { clientAddInformationPath, CommandHandler, getDefaultClient, mapClientToCreateClientRequest, RedirectorService } from '../../../../shared';

@Injectable({
  providedIn: 'root'
})
export class AddClientCommandHandlerService extends CommandHandler<AddClientCommand> {
  constructor(
    private readonly clientService: ClientService,
    private readonly shopDialog: ShopDialogManager,
    private readonly redirector: RedirectorService,
  ) {
    super();
  }

  dispatch(command: AddClientCommand): void {
    if (command.redirectAfter) {
      this.redirector.redirectTo(clientAddInformationPath(), { redirectTo: command.redirectAfter });
    }
    else {
      this.shopDialog.openClientChangeMenu(getDefaultClient()).afterClosed().pipe(
        take(1),
      ).subscribe(client => {
        if (client) {
          const req = mapClientToCreateClientRequest(client);
          this.clientService.createClient(req);
        }
      });
    }
  }

}
